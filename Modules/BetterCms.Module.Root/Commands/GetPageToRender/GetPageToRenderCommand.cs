using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<GetPageToRenderRequest, CmsRequestViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        private PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory;

        private PageStylesheetProjectionFactory pageStylesheetProjectionFactory;
        
        private readonly ICmsConfiguration cmsConfiguration;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory,
            PageStylesheetProjectionFactory pageStylesheetProjectionFactory, PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory,
            ICmsConfiguration cmsConfiguration)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageStylesheetProjectionFactory = pageStylesheetProjectionFactory;
            this.pageJavaScriptProjectionFactory = pageJavaScriptProjectionFactory;
            this.pageAccessor = pageAccessor;
            this.cmsConfiguration = cmsConfiguration;
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
            
            var page = pageQuery.FirstOrDefault();
            if (page == null)
            {
                return Redirect(request.PageUrl);
            }

            // Redirect user to login page, if page is inaccessible for public users
            // and user is not authenticated
            if (request.PreviewPageContentId == null && !request.IsAuthenticated && !page.IsPublic && !string.IsNullOrWhiteSpace(cmsConfiguration.LoginUrl))
            {
                // TODO: uncomment redirect to login form, when login form will be im
                return null;
                // return Redirect(cmsConfiguration.LoginUrl);
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

            return new CmsRequestViewModel(renderPageViewModel);
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

            List<IOption> options = new List<IOption>();
            options.AddRange(pageContent.Options);
            options.AddRange(pageContent.Content.ContentOptions.Where(f => !pageContent.Options.Any(g => g.Key.Trim().Equals(f.Key.Trim(), StringComparison.OrdinalIgnoreCase))));

            return pageContentProjectionFactory.Create(pageContent, contentToProject, options);
        }

        private IEnumerable<Page> GetPageFutureQuery(GetPageToRenderRequest request)
        {
            IQueryable<Page> query = Repository.AsQueryable<Page>()                                               
                                               .Fetch(f => f.Layout)
                                               .ThenFetchMany(f => f.LayoutRegions)
                                               .ThenFetch(f => f.Region)
                                               .Where(f => !f.IsDeleted);

            if (request.PageId == null)
            {
                query = query.Where(f => f.PageUrl.ToLower() == request.PageUrl.ToLowerInvariant());
            }
            else
            {
                query = query.Where(f => f.Id == request.PageId);
            }

            // If page is not published, page is not found
            if (!request.IsAuthenticated && request.PreviewPageContentId == null)
            {
                query = query.Where(f => f.IsPublished);
            }

            return query.ToFuture();
        }

        private IEnumerable<PageContent> GetPageContentFutureQuery(GetPageToRenderRequest request)
        {
            IQueryable<PageContent> pageContentsQuery =
                Repository.AsQueryable<PageContent>()                          
                          .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                          .FetchMany(f => f.Options);

            if (request.CanManageContent || request.PreviewPageContentId != null)
            {
                pageContentsQuery = pageContentsQuery.Fetch(f => f.Content).ThenFetchMany(f => f.History);
            }

            if (request.PageId == null)
            {
                pageContentsQuery = pageContentsQuery.Where(f => f.Page.PageUrl.ToLower() == request.PageUrl.ToLowerInvariant());
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

            pageContentsQuery = pageContentsQuery.Where(f => !f.IsDeleted && !f.Content.IsDeleted);

            return pageContentsQuery.ToFuture();
        }

        private CmsRequestViewModel Redirect(string redirectUrl)
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