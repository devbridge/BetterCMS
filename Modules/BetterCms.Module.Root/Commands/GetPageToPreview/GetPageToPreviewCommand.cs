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

using NHibernate.Criterion;
using NHibernate.SqlCommand;

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
            Page pageAlias = null;
            PageContent pageContentAlias = null;
            Root.Models.Content contentAlias = null;

            Page page = UnitOfWork.Session.QueryOver(() => pageAlias)
                              .JoinAlias(() => pageAlias.PageContents, () => pageContentAlias, JoinType.LeftOuterJoin, Restrictions.Where(() => !pageContentAlias.IsDeleted))
                              .JoinAlias(() => pageContentAlias.Content, () => contentAlias, JoinType.LeftOuterJoin, Restrictions.Where(() => !contentAlias.IsDeleted && contentAlias.Status == ContentStatus.Preview))
                              .Where(f => f.Id == request.PageId && !f.IsDeleted)
                              .Fetch(f => f.Layout)
                              .Eager.Fetch(f => f.Layout.LayoutRegions)
                              .Eager.Fetch(f => f.Layout.LayoutRegions[0].Region)
                              .Eager.Fetch(f => f.PageContents)
                              .Eager.Fetch(f => f.PageContents[0].Content)
                              .Eager.Fetch(f => f.PageContents[0].Options)
                              .Eager.Fetch(f => f.PageContents[0].Options[0].ContentOption)
                              .Eager.SingleOrDefault();
           
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