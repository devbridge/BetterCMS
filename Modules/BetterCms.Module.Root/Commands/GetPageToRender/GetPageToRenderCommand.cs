using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<GetPageToRenderRequest, CmsRequestViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        private PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory;

        private PageStylesheetProjectionFactory pageStylesheetProjectionFactory;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory,
            PageStylesheetProjectionFactory pageStylesheerProjectionFactory, PageJavaScriptProjectionFactory pageJavaScriptProjectionFactory)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageStylesheetProjectionFactory = pageStylesheerProjectionFactory;
            this.pageJavaScriptProjectionFactory = pageJavaScriptProjectionFactory;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request data with page data.</param>
        /// <returns>Executed command result.</returns>
        public CmsRequestViewModel Execute(GetPageToRenderRequest request)
        {
           var pageQuery = Repository.AsQueryable<Page>()    
                            .Where(f => !f.IsDeleted)         
                            .Fetch(f => f.Layout).ThenFetchMany(f => f.LayoutRegions).ThenFetch(f => f.Region)
                            .ToFuture();

           var pageContentsQuery = Repository.AsQueryable<PageContent>()
                              .Where(f => !f.IsDeleted)  
                              .Fetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                              .FetchMany(f => f.Options)
                              .ToFuture();

            if (request.PageId == null)
            {
                pageQuery = pageQuery.Where(f => !f.IsDeleted && f.PageUrl.ToLower() == request.PageUrl.ToLowerInvariant());
                pageContentsQuery = pageContentsQuery.Where(f => f.Page.PageUrl.ToLower() == request.PageUrl.ToLowerInvariant());
            }
            else
            {
                pageQuery = pageQuery.Where(f => !f.IsDeleted && f.Id == request.PageId);
                pageContentsQuery = pageContentsQuery.Where(f => f.Page.Id == request.PageId);
            }


            if (request.CanManageContent)
            {
                if (request.PreviewPageContentId != null)
                {
                    pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published || f.Content.Status == ContentStatus.Draft || f.Id == request.PreviewPageContentId.Value);
                }
                else
                {
                    pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published || f.Content.Status == ContentStatus.Draft);
                }
            }
            else
            {
                if (request.PreviewPageContentId != null)
                {
                    pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published || f.Id == request.PreviewPageContentId.Value);
                }
                else
                {
                    pageContentsQuery = pageContentsQuery.Where(f => f.Content.Status == ContentStatus.Published);
                }
            }

            var page = pageQuery.FirstOrDefault();
            if (page == null)
            {
                var redirect = pageAccessor.GetRedirect(request.PageUrl);
                if (!string.IsNullOrWhiteSpace(redirect))
                {
                    return new CmsRequestViewModel(new RedirectViewModel(redirect));
                }

                return null;
            }

            var pageContents = pageContentsQuery.ToList();
            var contentProjections = pageContents.Distinct().Select(f => pageContentProjectionFactory.Create(f)).ToList();
            
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

            // Styles
            var styles = new List<IStylesheetAccessor>();
            styles.Add(pageStylesheetProjectionFactory.Create(page));
            styles.AddRange(contentProjections);
            renderPageViewModel.Stylesheets = styles;

            // JavaScripts
            var js = new List<IJavaScriptAccessor>();
            js.Add(pageJavaScriptProjectionFactory.Create(page));
            js.AddRange(contentProjections);
            renderPageViewModel.JavaScripts = js;

            return new CmsRequestViewModel(renderPageViewModel);
        }
    }
}