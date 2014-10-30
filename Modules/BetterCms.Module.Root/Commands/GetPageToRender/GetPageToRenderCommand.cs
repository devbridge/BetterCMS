using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.Models.Extensions;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<GetPageToRenderRequest, CmsRequestViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory;

        private readonly PageStylesheetProjectionFactory pageStylesheetProjectionFactory;
        
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly RootModuleDescriptor rootModuleDescriptor;

        private readonly IOptionService optionService;

        private readonly IContentProjectionService contentProjectionService;
        
        private readonly IChildContentService childContentService;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageStylesheetProjectionFactory pageStylesheetProjectionFactory, 
            PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory,
            ICmsConfiguration cmsConfiguration, RootModuleDescriptor rootModuleDescriptor, IOptionService optionService,
            IContentProjectionService contentProjectionService, IChildContentService childContentService)
        {
            this.rootModuleDescriptor = rootModuleDescriptor;
            this.pageStylesheetProjectionFactory = pageStylesheetProjectionFactory;
            this.pageJavaScriptProjectionFactory = pageJavaScriptProjectionFactory;
            this.pageAccessor = pageAccessor;
            this.cmsConfiguration = cmsConfiguration;
            this.optionService = optionService;
            this.contentProjectionService = contentProjectionService;
            this.childContentService = childContentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public CmsRequestViewModel Execute(GetPageToRenderRequest request)
        {
            // Load the page
            var page = GetPage(request);
            if (page == null)
            {
                return FindRedirect(request.PageUrl);
            }

            // Load page contents
            var ids = new List<Guid> { page.Id };
            if (page.MasterPages != null)
            {
                ids.AddRange(page.MasterPages.Select(mp => mp.Master.Id).Distinct());
            }
            var pageContents = GetPageContents(ids.ToArray(), request);

            var childPagesList = new List<Page>();
            var renderPageViewModel = CreatePageViewModel(page, pageContents, page, request, childPagesList);

            // Notify about retrieved page.
            var result = Events.RootEvents.Instance.OnPageRetrieved(renderPageViewModel, page);

            switch (result)
            {
                case PageRetrievedEventResult.ForcePageNotFound:
                    return null;

                default:
                    return new CmsRequestViewModel(renderPageViewModel);
            }
        }

        private RenderPageViewModel CreatePageViewModel(Page renderingPage, List<PageContent> allPageContents, Page page, 
            GetPageToRenderRequest request, List<Page> childPagesList)
        {
            if (request.PreviewPageContentId == null && !request.IsAuthenticated && page.Status != PageStatus.Published)
            {
                throw new HttpException(403, "403 Access Forbidden");
            }

            // Preview and published pages can be accessible to users without content managing roles
            // Unpublished pages can be accessible only to content managers
            if (page.Status != PageStatus.Published && page.Status != PageStatus.Preview && !request.HasContentAccess)
            {
                if (!cmsConfiguration.Security.AccessControlEnabled)
                {
                    return null; // Force 404.
                }
            }

            RenderPageViewModel renderPageViewModel = new RenderPageViewModel(page);
            renderPageViewModel.CanManageContent = request.CanManageContent;
            renderPageViewModel.AreRegionsEditable = request.CanManageContent && !childPagesList.Any();

            if (page.Layout != null)
            {
                renderPageViewModel.LayoutPath = page.Layout.LayoutPath;
                renderPageViewModel.Options = GetMergedOptionValues(page.Layout.LayoutOptions, page.Options, childPagesList);
                renderPageViewModel.Regions = page.Layout.LayoutRegions
                    .Distinct()
                    .Select(f => new PageRegionViewModel
                        {
                            RegionId = f.Region.Id, 
                            RegionIdentifier = f.Region.RegionIdentifier
                        })
                    .ToList();
            }
            else if (page.MasterPage != null)
            {
                var masterPage = renderingPage.MasterPages.FirstOrDefault(p => p.Master.Id == page.MasterPage.Id);
                if (masterPage == null)
                {
                     throw new InvalidOperationException(string.Format("Cannot find a master page in master pages path collection for page {0}.", request.PageUrl));
                }

                childPagesList.Insert(0, page);
                renderPageViewModel.MasterPage = CreatePageViewModel(renderingPage, allPageContents, masterPage.Master, request, childPagesList);
                childPagesList.Remove(page);

                renderPageViewModel.Options = GetMergedOptionValues(new List<IOption>(), page.Options, childPagesList);
                renderPageViewModel.Regions = allPageContents
                    .Where(pc => pc.Page == page.MasterPage)
                    .SelectMany(pc => pc.Content.ContentRegions.Distinct())
                    .Select(cr => new PageRegionViewModel
                        {
                            RegionId = cr.Region.Id,
                            RegionIdentifier = cr.Region.RegionIdentifier
                        })
                    .ToList();
            }
            else
            {
                throw new InvalidOperationException(string.Format("Failed to load layout or master page for page {0}.", request.PageUrl));
            }

            var pageContents = allPageContents.Where(pc => pc.Page.Id == page.Id).Where(pc => pc.Parent == null || allPageContents.All(apc => apc.Id != pc.Parent.Id));
            var contentProjections = pageContents.Distinct()
                    .Select(f => contentProjectionService.CreatePageContentProjection(request.CanManageContent, f, allPageContents, null, request.PreviewPageContentId))
                    .Where(c => c != null).ToList();

            renderPageViewModel.Contents = contentProjections;
            renderPageViewModel.Metadata = pageAccessor.GetPageMetaData(page).ToList();

            if (page.AccessRules != null)
            {
                var list = page.AccessRules.Cast<IAccessRule>().ToList();
                list.RemoveDuplicates((a, b) => a.Id == b.Id ? 0 : -1);

                renderPageViewModel.AccessRules = list;
            }

            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                SetIsReadOnly(renderPageViewModel, renderPageViewModel.AccessRules);
            }
            renderPageViewModel.HasEditRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);

            // Add <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" /> if current view is in an edit mode.
            if (request.CanManageContent)
            {
                if (renderPageViewModel.Metadata == null)
                {
                    renderPageViewModel.Metadata = new List<IPageActionProjection>();
                }

                renderPageViewModel.Metadata.Insert(0, new MetaDataProjection("X-UA-Compatible", "IE=edge,chrome=1"));
            }

            // Attach styles.
            var styles = new List<IStylesheetAccessor>();
            styles.Add(pageStylesheetProjectionFactory.Create(page, renderPageViewModel.Options));
            styles.AddRange(contentProjections);
            renderPageViewModel.Stylesheets = styles;

            // Attach JavaScript includes.
            var js = new List<IJavaScriptAccessor>();
            js.Add(pageJavaScriptProjectionFactory.Create(page, renderPageViewModel.Options));
            js.AddRange(contentProjections);
            renderPageViewModel.JavaScripts = js;

            // TODO: Fix main.js and processor.js IE cache.
            renderPageViewModel.MainJsPath = string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.main.js");
            renderPageViewModel.RequireJsPath = VirtualPath.Combine(rootModuleDescriptor.JsBasePath, 
                                                                    cmsConfiguration.UseMinifiedResources 
                                                                        ? "bcms.require-2.1.5.min.js" 
                                                                        : "bcms.require-2.1.5.js");
            renderPageViewModel.Html5ShivJsPath = VirtualPath.Combine(rootModuleDescriptor.JsBasePath, "html5shiv.js");

            return renderPageViewModel;
        }
        
        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Page entity</returns>
        private Page GetPage(GetPageToRenderRequest request)
        {
            IQueryable<Page> query = (IQueryable<Page>)pageAccessor.GetPageQuery();
            query = query.Where(f => !f.IsDeleted);
            if (request.PageId == null)
            {
                var requestUrl = request.PageUrl.UrlHash();
                query = query.Where(f => f.PageUrlHash == requestUrl);
            }
            else
            {
                query = query.Where(f => f.Id == request.PageId);
            }

            // If page is not published, page is not found.
            if (!request.IsAuthenticated && request.PreviewPageContentId == null)
            {
                query = query.Where(f => f.Status == PageStatus.Published);
            }

            var page = GetPageQueryNew(query);
            
            pageAccessor.CachePage(page);

            return page;
        }

        private Page GetPageQueryOld(IQueryable<Page> query)
        {
            query = query
                .FetchMany(f => f.Options)
                .Fetch(f => f.PagesView)
                .Fetch(f => f.MasterPage)
                .Fetch(f => f.Layout)
                .ThenFetchMany(f => f.LayoutRegions)
                .ThenFetch(f => f.Region)
                .Fetch(f => f.Layout)
                .ThenFetchMany(f => f.LayoutOptions)

                // Fetch master page with reference to master page
                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master)
                .ThenFetchMany(f => f.AccessRules)

                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master)
                .ThenFetchMany(f => f.Options)

                // Fetch master page with reference to layout with it's regions and options
                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master)
                .ThenFetch(f => f.Layout)
                .ThenFetchMany(f => f.LayoutRegions)
                .ThenFetch(f => f.Region)

                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master)
                .ThenFetch(f => f.Layout)
                .ThenFetchMany(f => f.LayoutOptions);

            // Add access rules if access control is enabled.
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                query = query.FetchMany(f => f.AccessRules);
            }

            var page = query.ToList().FirstOrDefault();
            return page;
        }

        private Page GetPageQueryNew(IQueryable<Page> query)
        {
            var pageFutureQuery = query.Fetch(f => f.MasterPage)
                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master).ToFuture();

            var page = pageFutureQuery.ToList().FirstOrDefault();

            if (page != null)
            {
                var pagesIds = GetPageIds(page);
                var layoutsIds = GetLayoutIds(page);

                page.Options.ForEach(Repository.Detach);
                Repository.Detach(page.PagesView);
                Repository.Detach(page.Layout);

                // Pages options
                var options = Repository.AsQueryable<PageOption>().
                    Where(i => pagesIds.Contains(i.Page.Id)).ToFuture().ToList();

                // Pages views
                var pageViews = Repository.AsQueryable<PagesView>()
                    .Where(i => pagesIds.Contains(i.Page.Id)).ToFuture().ToList();
                
                // Layouts
                var layouts = Repository.AsQueryable<Layout>()
                    .Where(i => layoutsIds.Contains(i.Id))
                    .FetchMany(i => i.LayoutRegions)
                    .ThenFetch(i => i.Region).ToFuture().ToList();
                layouts.ForEach(Repository.Detach);
                var layoutOptions = Repository.AsQueryable<LayoutOption>()
                    .Where(i => layoutsIds.Contains(i.Layout.Id)).ToFuture().ToList();
                layouts.ForEach(l => l.LayoutOptions = layoutOptions.Where(lo => lo.Layout.Id == l.Id).ToList());

                page = SetPageData(page, options, pageViews, layouts);

                return page;
            }

            return null;
        }

        private Page SetPageData(Page page, List<PageOption> options,
            List<PagesView> pagesViews, List<Layout> layouts)
        {
            page.Options = options.Where(i => i.Page.Id == page.Id).ToList();
            page.PagesView = pagesViews.FirstOrDefault(i => i.Page.Id == page.Id);
            if (page.Layout != null)
            {
                page.Layout = layouts.FirstOrDefault(i => i.Id == page.Layout.Id);
            }

            foreach (var masterPage in page.MasterPages)
            {
                masterPage.Master.Options.ForEach(Repository.Detach);
                Repository.Detach(masterPage.Master.PagesView);

                masterPage.Master.Options = options.Where(i => i.Page.Id == masterPage.Master.Id).ToList();
                masterPage.Master.PagesView = pagesViews.FirstOrDefault(i => i.Page.Id == masterPage.Master.Id);
                if (masterPage.Master.Layout != null)
                {
                    masterPage.Master.Layout = layouts.FirstOrDefault(i => i.Id == masterPage.Master.Layout.Id);
                }
            }

            if (page.MasterPage != null)
            {
                page.MasterPage.Options = options.Where(i => i.Page.Id == page.MasterPage.Id).ToList();
                page.MasterPage.PagesView = pagesViews.FirstOrDefault(i => i.Page.Id == page.MasterPage.Id);
                if (page.MasterPage.Layout != null)
                {
                    page.MasterPage.Layout = layouts.FirstOrDefault(i => i.Id == page.MasterPage.Layout.Id);
                }
            }

            return page;
        }

        private List<Guid> GetPageIds(Page page)
        {
            var pagesIds = new List<Guid> { page.Id };
            foreach (var masterPage in page.MasterPages)
            {
                pagesIds.Add(masterPage.Master.Id);
            }
            
            return pagesIds.Distinct().ToList();
        }

        private List<Guid> GetLayoutIds(Page page)
        {
            var layoutIds = new List<Guid>();
            if (page.Layout != null)
            {
                layoutIds.Add(page.Layout.Id);
            }
            foreach (var masterPage in page.MasterPages)
            {
                if (masterPage.Master.Layout != null)
                {
                    layoutIds.Add(masterPage.Master.Layout.Id);
                }
            }
            
            return layoutIds.Distinct().ToList();
        }

        /// <summary>
        /// Gets the page contents.
        /// </summary>
        /// <param name="pageIds">The page ids.</param>
        /// <param name="request">The request.</param>
        /// <returns>The list of page contents</returns>
        private List<PageContent> GetPageContents(Guid[] pageIds, GetPageToRenderRequest request)
        {
            IQueryable<PageContent> pageContentsQuery = Repository.AsQueryable<PageContent>();

            pageContentsQuery = pageContentsQuery.Where(f => pageIds.Contains(f.Page.Id));

            if (request.PreviewPageContentId != null)
            {
                pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published || f.Content.Status == ContentStatus.Draft || f.Id == request.PreviewPageContentId.Value);
            }
            else if (request.CanManageContent)
            {
                pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published || f.Content.Status == ContentStatus.Draft);               
            }
            else
            {
                pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published);
            }

            pageContentsQuery = pageContentsQuery.Where(f => !f.IsDeleted && !f.Content.IsDeleted && !f.Page.IsDeleted);

            var pageContents = GetPageContentsQueryNew(pageContentsQuery, request);

            childContentService.RetrieveChildrenContentsRecursively(request.CanManageContent, pageContents.Select(pc => pc.Content).Distinct().ToList());

            return pageContents;
        }

        private List<PageContent> GetPageContentsQueryOld(IQueryable<PageContent> pageContentsQuery, GetPageToRenderRequest request)
        {
            pageContentsQuery = pageContentsQuery.Where(f => !f.IsDeleted && !f.Content.IsDeleted && !f.Page.IsDeleted);

            pageContentsQuery = pageContentsQuery
                .Fetch(f => f.Content)
                .ThenFetchMany(f => f.ContentOptions)

                .FetchMany(f => f.Options)

                .Fetch(f => f.Content)
                .ThenFetchMany(f => f.ContentRegions)
                .ThenFetch(f => f.Region)

                // Fetch child contents
                .Fetch(f => f.Content)
                .ThenFetchMany(f => f.ChildContents);

            if (request.CanManageContent || request.PreviewPageContentId != null)
            {
                pageContentsQuery = pageContentsQuery
                    .Fetch(f => f.Content)
                    .ThenFetchMany(f => f.History)
                    .ThenFetchMany(f => f.ChildContents);
            }

            var pageContents = pageContentsQuery.ToList();

            return pageContents;
        }

        private List<PageContent> GetPageContentsQueryNew(IQueryable<PageContent> pageContentsQuery, GetPageToRenderRequest request)
        {
            var pageContents = pageContentsQuery.ToFuture().ToList();
            var contentsIds = pageContents.Where(i => i.Content != null).Select(i => i.Content.Id).Distinct().ToList();
            var pageContentsIds = pageContents.Select(i => i.Id).Distinct().ToList();

            foreach (var pageContent in pageContents)
            {
                pageContent.Options.ForEach(Repository.Detach);
                Repository.Detach(pageContent.Content);
            }

            var pageContentOptions = Repository.AsQueryable<PageContentOption>()
                .Where(i => pageContentsIds.Contains(i.PageContent.Id)).ToFuture().ToList();

            var contents = Repository.AsQueryable<Models.Content>()
                .Where(i => contentsIds.Contains(i.Id)).ToFuture().ToList();

            foreach (var content in contents)
            {
                content.ContentOptions.ForEach(Repository.Detach);
                content.ContentRegions.ForEach(Repository.Detach);
                content.ChildContents.ForEach(Repository.Detach);
                content.History.ForEach(Repository.Detach);
            }

            var contentOptions = Repository.AsQueryable<ContentOption>()
                .Where(i => contentsIds.Contains(i.Content.Id)).ToFuture().ToList();

            var contentRegions = Repository.AsQueryable<ContentRegion>()
                .Where(i => contentsIds.Contains(i.Content.Id))
                .Fetch(i => i.Region).ToFuture().ToList();

            var childContents = Repository.AsQueryable<ChildContent>()
                .Where(i => contentsIds.Contains(i.Parent.Id)).ToFuture().ToList();

            List<Models.Content> contentHistories = new List<Models.Content>();
            if (request.CanManageContent || request.PreviewPageContentId != null)
            {
                contentHistories =
                    Repository.AsQueryable<Models.Content>()
                    .Where(i => contentsIds.Contains(i.Original.Id))
                    .FetchMany(i => i.ChildContents).ToFuture().ToList();
            }

            // Fill contents data
            foreach (var content in contents)
            {
                content.ContentOptions = contentOptions.Where(i => i.Content.Id == content.Id).ToList();
                content.ContentRegions = contentRegions.Where(i => i.Content.Id == content.Id).ToList();
                content.ChildContents = childContents.Where(i => i.Parent.Id == content.Id).ToList();
                if (request.CanManageContent || request.PreviewPageContentId != null)
                {
                    content.History = contentHistories.Where(i => i.Original.Id == content.Id).ToList();
                }
            }

            // Fill page content data
            foreach (var pageContent in pageContents)
            {
                pageContent.Options = pageContentOptions.Where(i => i.PageContent.Id == pageContent.Id).ToList();
                pageContent.Content = contents.FirstOrDefault(i => i.Id == pageContent.Content.Id);
            }

            return pageContents;
        }

        /// <summary>
        /// Finds the redirect.
        /// </summary>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <returns>Redirect view model</returns>
        private CmsRequestViewModel FindRedirect(string redirectUrl)
        {
            var redirect = pageAccessor.GetRedirect(redirectUrl);
            if (!string.IsNullOrWhiteSpace(redirect))
            {
                return new CmsRequestViewModel(new RedirectViewModel(redirect));
            }

            return null;
        }

        /// <summary>
        /// Gets the merged option values.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <param name="childrenPages">The children pages.</param>
        /// <returns>Merged option values</returns>
        private List<IOptionValue> GetMergedOptionValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues, IList<Page> childrenPages)
        {
            var mergedOptions = new List<IOptionValue>();

            foreach (var option in optionService.GetMergedOptionValues(options, optionValues))
            {
                if (!childrenPages.Any(co => co.Options.Any(o => o.Key == option.Key 
                    && o.Type == option.Type 
                    && ((o.CustomOption == null && option.CustomOption == null) 
                        || (o.CustomOption != null 
                            && option.CustomOption != null 
                            && o.CustomOption.Identifier == option.CustomOption.Identifier)))))
                {
                    mergedOptions.Add(option);
                }
            }

            return mergedOptions;
        }
    }
}