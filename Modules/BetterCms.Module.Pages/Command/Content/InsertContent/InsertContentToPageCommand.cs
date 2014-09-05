using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataAccess.DataContext.Fetching;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.ViewModels.Content;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;
using BetterCms.Module.Root.Services;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageCommand : CommandBase, ICommand<InsertContentToPageRequest, InsertContentToPageResultViewModel>
    {
        private readonly IContentService contentService;
        private readonly PageContentProjectionFactory projectionFactory;
        private readonly IChildContentService childContentService;

        public InsertContentToPageCommand(IContentService contentService, PageContentProjectionFactory projectionFactory,
            IChildContentService childContentService)
        {
            this.contentService = contentService;
            this.projectionFactory = projectionFactory;
            this.childContentService = childContentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public InsertContentToPageResultViewModel Execute(InsertContentToPageRequest request)
        {
            UnitOfWork.BeginTransaction();
            
            var page = Repository.AsProxy<Root.Models.Page>(request.PageId);
            var region = Repository.AsProxy<Region>(request.RegionId);
            var content = Repository
                .AsQueryable<Root.Models.Content>(c => c.Id == request.ContentId)
                .FetchMany(c => c.ContentRegions)
                .ToList()
                .FirstOne();
            
            PageContent parentPageContent = null;
            if (request.ParentPageContentId.HasValue && !request.ParentPageContentId.Value.HasDefaultValue())
            {
                parentPageContent = Repository.AsProxy<PageContent>(request.ParentPageContentId.Value);
            }

            var pageContent = new PageContent
                                {
                                    Page = page,
                                    Content = content,
                                    Region = region,
                                    Parent = parentPageContent
                                };
            pageContent.Order = contentService.GetPageContentNextOrderNumber(request.PageId, request.ParentPageContentId);


            Repository.Save(pageContent);
            UnitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnPageContentInserted(pageContent);

            var accessor = projectionFactory.GetAccessorForType(content);

            var model = new InsertContentToPageResultViewModel
                {
                    PageContentId = pageContent.Id,
                    ContentId = content.Id,
                    RegionId = request.RegionId,
                    PageId = request.PageId,
                    DesirableStatus = content.Status,
                    Title = content.Name,
                    ContentVersion = content.Version,
                    PageContentVersion = pageContent.Version,
                    ContentType = accessor != null ? accessor.GetContentWrapperType() : null
                };

            if (request.IncludeChildRegions)
            {
                model.Regions = new List<PageContentChildRegionViewModel>();

                if (content.ContentRegions != null)
                {
                    foreach (var contentRegion in content.ContentRegions.Distinct())
                    {
                        model.Regions.Add(new PageContentChildRegionViewModel(contentRegion));
                    }
                }

                var childContents = childContentService.RetrieveChildrenContentsRecursively(true, new[] { content.Id });
                if (childContents != null)
                {
                    foreach (var childContent in childContents)
                    {
                        if (childContent.Child.ContentRegions != null)
                        {
                            foreach (var contentRegion in childContent.Child.ContentRegions.Distinct())
                            {
                                model.Regions.Add(new PageContentChildRegionViewModel(contentRegion));
                            }
                        }
                    }
                }

                model.Regions = model.Regions.GroupBy(r => r.RegionIdentifier).Select(r => r.First()).ToList();
            }

            return model;
        }
    }
}