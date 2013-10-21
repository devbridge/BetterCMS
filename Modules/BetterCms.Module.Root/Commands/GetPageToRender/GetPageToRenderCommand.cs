using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
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
using BetterCms.Module.Root.ViewModels.Option;
using BetterCms.Module.Root.Models.Extensions;
using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<GetPageToRenderRequest, CmsRequestViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        private readonly PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory;

        private readonly PageStylesheetProjectionFactory pageStylesheetProjectionFactory;
        
        private readonly ICmsConfiguration cmsConfiguration;

        private readonly RootModuleDescriptor rootModuleDescriptor;
        
        private readonly IOptionService optionService;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory,
            PageStylesheetProjectionFactory pageStylesheetProjectionFactory, PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory,
            ICmsConfiguration cmsConfiguration, RootModuleDescriptor rootModuleDescriptor, IOptionService optionService)
        {
            this.rootModuleDescriptor = rootModuleDescriptor;
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageStylesheetProjectionFactory = pageStylesheetProjectionFactory;
            this.pageJavaScriptProjectionFactory = pageJavaScriptProjectionFactory;
            this.pageAccessor = pageAccessor;
            this.cmsConfiguration = cmsConfiguration;
            this.optionService = optionService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request data with page data.</param>
        /// <returns>Executed command result.</returns>
        public CmsRequestViewModel Execute(GetPageToRenderRequest request)
        {
            var pageQuery = GetPageFutureQuery(request);
            var pageContentsQuery = GetPageContentFutureQuery(request);
            
            var page = pageQuery.ToList().FirstOrDefault();

            if (page == null)
            {
                return FindRedirect(request.PageUrl);
            }

            if (request.PreviewPageContentId == null && !request.IsAuthenticated && page.Status != PageStatus.Published)
            {
                throw new HttpException(403, "403 Access Forbidden");
            }

            if (page.Status != PageStatus.Published && !request.CanManageContent)
            {
                if (!cmsConfiguration.Security.AccessControlEnabled)
                {
                    return null; // Force 404.
                }
            }

            var pageContents = pageContentsQuery.ToList();
            var contentProjections = pageContents.Distinct().Select(f => CreatePageContentProjection(request, f)).Where(c => c != null).ToList();

            RenderPageViewModel renderPageViewModel = new RenderPageViewModel(page);
            renderPageViewModel.CanManageContent = request.CanManageContent;
            renderPageViewModel.LayoutPath = page.Layout.LayoutPath;

            renderPageViewModel.Regions =
                page.Layout.LayoutRegions.Distinct().Select(f => new PageRegionViewModel
                                                      {
                                                          RegionId = f.Region.Id,
                                                          RegionIdentifier = f.Region.RegionIdentifier
                                                      })
                                                      .ToList();

            renderPageViewModel.Contents = contentProjections;
            renderPageViewModel.Metadata = pageAccessor.GetPageMetaData(page).ToList();
            renderPageViewModel.Options = optionService.GetMergedOptionValues(page.Layout.LayoutOptions, page.Options).ToList();

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
            styles.Add(pageStylesheetProjectionFactory.Create(page));
            styles.AddRange(contentProjections);
            renderPageViewModel.Stylesheets = styles;

            // Attach JavaScript includes.
            var js = new List<IJavaScriptAccessor>();
            js.Add(pageJavaScriptProjectionFactory.Create(page));
            js.AddRange(contentProjections);

            renderPageViewModel.JavaScripts = js;
            // TODO: Fix main.js and processor.js IE cache.
            renderPageViewModel.MainJsPath = string.Format(RootModuleConstants.AutoGeneratedJsFilePathPattern, "bcms.main.js");
            renderPageViewModel.RequireJsPath = VirtualPath.Combine(rootModuleDescriptor.JsBasePath, 
                                                                    cmsConfiguration.UseMinifiedResources 
                                                                        ? "bcms.require-2.1.5.min.js" 
                                                                        : "bcms.require-2.1.5.js");
            renderPageViewModel.Html5ShivJsPath = VirtualPath.Combine(rootModuleDescriptor.JsBasePath, "html5shiv.js");

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

        private PageContentProjection CreatePageContentProjection(GetPageToRenderRequest request, PageContent pageContent)
        {
            Models.Content contentToProject = null;
            
            if (request.PreviewPageContentId != null && request.PreviewPageContentId.Value == pageContent.Id)
            {
                // Looks for the preview content version first.
                if (pageContent.Content.Status == ContentStatus.Preview)
                {
                    contentToProject = pageContent.Content;
                }
                else
                {
                    contentToProject = pageContent.Content.History.FirstOrDefault(f => f.Status == ContentStatus.Preview);
                }
            }

            if (contentToProject == null && (request.CanManageContent || request.PreviewPageContentId != null))
            {
                // Look for the draft content version if we are in the edit or preview mode.
                if (pageContent.Content.Status == ContentStatus.Draft)
                {
                    contentToProject = pageContent.Content;
                }
                else
                {
                    contentToProject = pageContent.Content.History.FirstOrDefault(f => f.Status == ContentStatus.Draft);
                }
            }
            
            if (contentToProject == null && pageContent.Content.Status == ContentStatus.Published)
            {
                IHtmlContent htmlContent = pageContent.Content as IHtmlContent;
                if (!request.CanManageContent && htmlContent != null && (DateTime.Now < htmlContent.ActivationDate || (htmlContent.ExpirationDate.HasValue && htmlContent.ExpirationDate.Value < DateTime.Now)))
                {
                    // Invisible for user because of activation dates.
                    return null;
                }

                // Otherwise take published version.
                contentToProject = pageContent.Content;
            }

            if (contentToProject == null)
            {
                throw new CmsException(string.Format("A content version was not found to project on the page. PageContent={0}; Request={1};", pageContent, request));
            }

            var options = optionService.GetMergedOptionValues(contentToProject.ContentOptions, pageContent.Options);
            
            return pageContentProjectionFactory.Create(pageContent, contentToProject, options);
        }

        private IEnumerable<Page> GetPageFutureQuery(GetPageToRenderRequest request)
        {
            IQueryable<Page> query = Repository.AsQueryable<Page>().Where(f => !f.IsDeleted);

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

            // Add fetched entities.
            query = query
                .Fetch(f => f.Layout)
                .ThenFetchMany(f => f.LayoutRegions)
                .ThenFetch(f => f.Region);

            // Add access rules if access control is enabled.
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                query = query.FetchMany(f => f.AccessRules);
            }

            return query.ToFuture();
        }

        private IEnumerable<PageContent> GetPageContentFutureQuery(GetPageToRenderRequest request)
        {
            IQueryable<PageContent> pageContentsQuery =
                Repository.AsQueryable<PageContent>();

            if (request.PageId == null)
            {
                var requestUrl = request.PageUrl.UrlHash();
                pageContentsQuery = pageContentsQuery.Where(f => f.Page.PageUrlHash == requestUrl);
            }
            else
            {
                pageContentsQuery = pageContentsQuery.Where(f => f.Page.Id == request.PageId);
            }

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

            pageContentsQuery = pageContentsQuery
                .Fetch(f => f.Content)
                .ThenFetchMany(f => f.ContentOptions)
                .FetchMany(f => f.Options);

            if (request.CanManageContent || request.PreviewPageContentId != null)
            {
                pageContentsQuery = pageContentsQuery.Fetch(f => f.Content).ThenFetchMany(f => f.History);
            }

            return pageContentsQuery.ToFuture();
        }
        
        private CmsRequestViewModel FindRedirect(string redirectUrl)
        {
            var redirect = pageAccessor.GetRedirect(redirectUrl);
            if (!string.IsNullOrWhiteSpace(redirect))
            {
                return new CmsRequestViewModel(new RedirectViewModel(redirect));
            }

            return null;
        }
    }
}