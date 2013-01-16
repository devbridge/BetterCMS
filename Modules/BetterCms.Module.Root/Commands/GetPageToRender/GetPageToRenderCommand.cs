using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.ViewModels.Cms;

using NHibernate;
using NHibernate.Criterion;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderCommand : CommandBase, ICommand<GetPageToRenderRequest, CmsRequestViewModel>
    {
        private IPageAccessor pageAccessor;

        private PageContentProjectionFactory pageContentProjectionFactory;

        public GetPageToRenderCommand(IPageAccessor pageAccessor, PageContentProjectionFactory pageContentProjectionFactory)
        {
            this.pageContentProjectionFactory = pageContentProjectionFactory;
            this.pageAccessor = pageAccessor;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request data with page data.</param>
        /// <returns>Executed command result.</returns>
        public CmsRequestViewModel Execute(GetPageToRenderRequest request)
        {
            Page pageAlias = null;
            ICriterion pageFilter = Restrictions.Eq(
                NHibernate.Criterion.Projections.SqlFunction("lower", NHibernateUtil.String, NHibernate.Criterion.Projections.Property(() => pageAlias.PageUrl)),
                request.VirtualPath.ToLowerInvariant());


            Page page;
            if (request.PreviewPageContentId == null)
            {
                page = UnitOfWork.Session.QueryOver(() => pageAlias)
                              .Where(pageFilter)
                              .Fetch(f => f.Layout)
                              .Eager.Fetch(f => f.Layout.LayoutRegions)
                              .Eager.Fetch(f => f.Layout.LayoutRegions[0].Region)
                              .Eager.Fetch(f => f.PageContents)
                              .Eager.Fetch(f => f.PageContents[0].Content)
                              .Eager.Fetch(f => f.PageContents[0].Options)
                              .Eager.Fetch(f => f.PageContents[0].Options[0].ContentOption)
                              .Eager.SingleOrDefault();
            }
            else
            {
                page = UnitOfWork.Session.QueryOver(() => pageAlias)
                              .Where(pageFilter)
                              .Fetch(f => f.Layout)
                              .Eager.Fetch(f => f.Layout.LayoutRegions)
                              .Eager.Fetch(f => f.Layout.LayoutRegions[0].Region)
                              .Eager.Fetch(f => f.PageContents)
                              .Eager.Fetch(f => f.PageContents[0].Content)
                              .Eager.Fetch(f => f.PageContents[0].Options)
                              .Eager.Fetch(f => f.PageContents[0].Options[0].ContentOption)
                              .Eager.SingleOrDefault();
            }
            if (page == null)
            {
                var redirect = pageAccessor.GetRedirect(request.VirtualPath);
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