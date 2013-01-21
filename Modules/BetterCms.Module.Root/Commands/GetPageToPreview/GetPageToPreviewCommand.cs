using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Commands.GetPageToPreview
{
    public class GetPageToPreviewCommand : CommandBase, ICommand<GetPageToPreviewRequest, RenderPageViewModel>
    {
        private readonly IPageAccessor pageAccessor;

        private readonly PageContentProjectionFactory pageContentProjectionFactory;

        public GetPageToPreviewCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request data with page data.</param>
        /// <returns>Executed command result.</returns>
        public RenderPageViewModel Execute(GetPageToPreviewRequest request)
        {
            Page page = Repository.AsQueryable<Page>()
                            .Where(f => !f.IsDeleted)
                            .Where(f => f.Id == request.PageId)
                            .Fetch(f => f.Layout).ThenFetchMany(f => f.LayoutRegions).ThenFetch(f => f.Region)
                            .FetchMany(f => f.PageContents).ThenFetch(f => f.Content).ThenFetchMany(f => f.ContentOptions)
                            .FetchMany(f => f.PageContents).ThenFetchMany(f => f.Options)
                            .ToList()
                            .FirstOrDefault();
           
            if (page == null)
            {
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
            
            return renderPageViewModel;
        }
    }
}