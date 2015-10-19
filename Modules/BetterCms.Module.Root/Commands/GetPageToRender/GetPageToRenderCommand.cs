using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Cms;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.ViewModels.Security;
using BetterCms.Module.Root.Views.Language;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Mvc.Extensions;

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

                var masterOptions = new List<PageOption>();
                foreach (var optionValue in renderingPage.MasterPages
                    .SelectMany(mp => mp.Master.Options.Where(optionValue => !masterOptions.Contains(optionValue))))
                {
                    masterOptions.Add(optionValue);
                }

                renderPageViewModel.Options = GetMergedOptionValues(masterOptions, page.Options, childPagesList);
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
                    .Select(f => contentProjectionService.CreatePageContentProjection(request.CanManageContent, f, allPageContents, null, request.PreviewPageContentId, renderPageViewModel.LanguageId))
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

            var page = CollectPageData(query);
            
            pageAccessor.CachePage(page);

            return page;
        }

        private Page CollectPageData(IQueryable<Page> query)
        {
            var page = query
                .Fetch(f => f.PagesView)
                .Fetch(f => f.Language)
                .Fetch(f => f.MasterPage)
                .FetchMany(f => f.MasterPages)
                .ThenFetch(f => f.Master)
                .ToList()
                .FirstOrDefault();

            if (page != null)
            {
                FakeDetachPage(page);

                var pagesIds = GetPageIds(page);
                var layoutsIds = GetLayoutIds(page);

                // Pages options
                var optionsFuture = Repository.AsQueryable<PageOption>().
                    Where(i => pagesIds.Contains(i.Page.Id)).ToFuture();

                // Pages access rules
                var accessRulesFuture = Repository
                   .AsQueryable<Page>()
                   .Where(x => pagesIds.Contains(x.Id) && !x.IsDeleted)
                   .Select(x => new PageAccessRulesModel { AccessRules = x.AccessRules, PageId = x.Id })
                   .ToFuture();
                
                // Layouts
                var layoutsQuery = Repository.AsQueryable<Layout>()
                    .Where(i => layoutsIds.Contains(i.Id))
                    .FetchMany(i => i.LayoutRegions)
                    .ThenFetch(i => i.Region).ToFuture();
                var layoutOptions = Repository.AsQueryable<LayoutOption>()
                    .Where(i => layoutsIds.Contains(i.Layout.Id)).ToFuture();

                var layouts = layoutsQuery.ToList();
                layouts.ToList().ForEach(Repository.Detach);
                layouts.ForEach(l => l.LayoutOptions = layoutOptions.Where(lo => lo.Layout.Id == l.Id).ToList());

                page = SetPageData(page, optionsFuture.ToList(), layouts, accessRulesFuture.ToList());

                return page;
            }

            return null;
        }

        private Page SetPageData(Page page, List<PageOption> options, List<Layout> layouts, List<PageAccessRulesModel> accessRules)
        {
            SetPageOptions(page, options);
            SetPageAcessRules(page, accessRules);
            SetPageLayout(page, layouts);

            if (page.MasterPages != null)
            {
                foreach (var masterPage in page.MasterPages)
                {
                    FakeDetachPage(masterPage.Master);

                    SetPageOptions(masterPage.Master, options);
                    SetPageAcessRules(masterPage.Master, accessRules);
                    SetPageLayout(masterPage.Master, layouts);
                }
            }

            if (page.MasterPage != null)
            {
                FakeDetachPage(page.MasterPage);

                SetPageOptions(page.MasterPage, options);
                SetPageAcessRules(page.MasterPage, accessRules);
                SetPageLayout(page.MasterPage, layouts);
            }

            return page;
        }

        private void FakeDetachPage(Page page)
        {
            // Instead of detaching  full object, detaching only properies, which will be loaded using future queries
            // Cannot detach whole page, because it should be up-to-date:
            // there are many items, which are loaded using lazy load later (images, pageTags, etc)
            page.Options = new List<PageOption>();
            page.AccessRules = new List<AccessRule>();
        }

        private void SetPageLayout(Page page, List<Layout> layouts)
        {
            if (page.Layout != null)
            {
                page.Layout = layouts.FirstOrDefault(i => i.Id == page.Layout.Id);
            }
        }

        private void SetPageOptions(Page page, List<PageOption> options)
        {
            page.Options = options.Where(i => i.Page.Id == page.Id).ToList();
        }

        private void SetPageAcessRules(Page page, List<PageAccessRulesModel> accessRules)
        {
            var pageAccessRules = accessRules.FirstOrDefault(ar => ar.PageId == page.Id);
            page.AccessRules = pageAccessRules == null || pageAccessRules.AccessRules == null
                ? new List<AccessRule>() : pageAccessRules.AccessRules.ToList();
        }

        private List<Guid> GetPageIds(Page page)
        {
            var pagesIds = new List<Guid> { page.Id };
            if (page.MasterPages != null)
            {
                foreach (var masterPage in page.MasterPages)
                {
                    pagesIds.Add(masterPage.Master.Id);
                }
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

            var pageContents = CollectPageContentsData(pageContentsQuery, request);

            childContentService.RetrieveChildrenContentsRecursively(request.CanManageContent, pageContents.Select(pc => pc.Content).Distinct().ToList());

            return pageContents;
        }

        private List<PageContent> CollectPageContentsData(IEnumerable<PageContent> pageContentsQuery, GetPageToRenderRequest request)
        {
            var pageContents = pageContentsQuery.ToList();
            var contentsIds = pageContents.Where(i => i.Content != null).Select(i => i.Content.Id).Distinct().ToList();
            var pageContentsIds = pageContents.Select(i => i.Id).Distinct().ToList();

            foreach (var pageContent in pageContents)
            {
                pageContent.Options = new List<PageContentOption>();
            }

            var pageContentOptions = Repository.AsQueryable<PageContentOption>()
                .Where(i => pageContentsIds.Contains(i.PageContent.Id)).ToFuture();

            var contentOptions = Repository.AsQueryable<ContentOption>()
                .Where(i => contentsIds.Contains(i.Content.Id)).FetchMany(co => co.Translations).ToFuture();

            var contentRegions = Repository.AsQueryable<ContentRegion>()
                .Where(i => contentsIds.Contains(i.Content.Id))
                .Fetch(i => i.Region).ToFuture();

            IEnumerable<Models.Content> contentHistories = new List<Models.Content>();
            if (request.CanManageContent || request.PreviewPageContentId != null)
            {
                contentHistories =
                    Repository.AsQueryable<Models.Content>()
                    .Where(i => contentsIds.Contains(i.Original.Id) && i.Status != ContentStatus.Archived)
                    .FetchMany(i => i.ChildContents).ToFuture();
            }

            var childContents = Repository.AsQueryable<ChildContent>()
                .Where(i => contentsIds.Contains(i.Parent.Id)).ToFuture();

            var contents = Repository.AsQueryable<Models.Content>()
                .Where(i => contentsIds.Contains(i.Id)).ToFuture()
                .ToList()
                .Select(c => Repository.UnProxy(c)).ToList();

            FakeDetachPageContent(contents);

            // Fill contents data
            SetContentsData(contents, contentOptions.ToList(), contentRegions.ToList(),
                childContents.ToList(), contentHistories.ToList());

            // Fill page content data
            SetPageContentsData(pageContents, pageContentOptions.ToList(), contents);

            return pageContents;
        }

        private void FakeDetachPageContent(IList<Models.Content> contents)
        {
            foreach (var content in contents)
            {
                content.ContentOptions = new List<ContentOption>();
                content.ContentRegions = new List<ContentRegion>();
                content.ChildContents = new List<ChildContent>();
                content.History = new List<Models.Content>();
            }
        }

        private void SetContentsData(IList<Models.Content> contents, IList<ContentOption> contentOptions,
            IList<ContentRegion> contentRegions, IList<ChildContent> childContents, IList<Models.Content> contentHistories)
        {
            foreach (var content in contents)
            {
                content.ContentOptions = contentOptions.Where(i => i.Content.Id == content.Id).ToList();
                content.ContentRegions = contentRegions.Where(i => i.Content.Id == content.Id).ToList();
                content.ChildContents = childContents.Where(i => i.Parent.Id == content.Id).ToList();
                if (contentHistories.Count > 0)
                {
                    content.History = contentHistories.Where(i => i.Original.Id == content.Id).ToList();
                }
            }
        }

        private void SetPageContentsData(IList<PageContent> pageContents,
            IList<PageContentOption> pageContentOptions, IList<Models.Content> contents)
        {
            foreach (var pageContent in pageContents)
            {
                pageContent.Options = pageContentOptions.Where(i => i.PageContent.Id == pageContent.Id).ToList();
                pageContent.Content = contents.FirstOrDefault(i => i.Id == pageContent.Content.Id);
            }
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
        private IList<IOptionValue> GetMergedOptionValues(IEnumerable<IOptionEntity> options, IEnumerable<IOptionEntity> optionValues, IList<Page> childrenPages)
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

        private class PageAccessRulesModel
        {
            public IEnumerable<AccessRule> AccessRules { get; set; }
            public Guid PageId { get; set; }
        }
    }
}