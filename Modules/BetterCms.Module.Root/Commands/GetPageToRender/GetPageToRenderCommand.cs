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

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<string, CmsRequestViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="virtualPath">The request data with page data.</param>
        /// <returns>Executed command result.</returns>
        public CmsRequestViewModel Execute(string virtualPath)
        {
            Page page = Repository.AsQueryable<Page>()
                            .Where(f => !f.IsDeleted)
                            .Where(f => f.PageUrl.ToLower() == virtualPath.ToLowerInvariant())
                            .Fetch(f => f.Layout).ThenFetchMany(f => f.LayoutRegions).ThenFetch(f => f.Region)                            
                            .FetchMany(f => f.PageContents).ThenFetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                            .FetchMany(f => f.PageContents).ThenFetchMany(f => f.Options)
                            .ToList()
                            .FirstOrDefault();
           
            if (page == null)
            {
                var redirect = pageAccessor.GetRedirect(virtualPath);
                if (!string.IsNullOrWhiteSpace(redirect))
                {
                    return new CmsRequestViewModel(new RedirectViewModel(redirect));
                }

                return null;
            }

            var contentProjections = page.PageContents.Distinct().Select(f => pageContentProjectionFactory.Create(f)).ToList();

            RenderPageViewModel renderPageViewModel = new RenderPageViewModel(page);
            
            renderPageViewModel.LayoutPath = page.Layout.LayoutPath;

            renderPageViewModel.Regions =
                page.Layout.LayoutRegions.Distinct().Select(f => new PageRegionViewModel
                                                      {
                                                          RegionId = f.Region.Id,
                                                          RegionIdentifier = f.Region.RegionIdentifier
                                                      })
                                                      .ToList();

            renderPageViewModel.Contents = contentProjections;
            renderPageViewModel.Stylesheets = new List<IStylesheetAccessor>(contentProjections);
            renderPageViewModel.Metadata = pageAccessor.GetPageMetaData(page).ToList();
            
            return new CmsRequestViewModel(renderPageViewModel);
        }
    }
}