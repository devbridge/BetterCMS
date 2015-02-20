using System;
using System.Linq;

using BetterCms.Module.Api.Extensions;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    /// <summary>
    /// Default page content CRUD service.
    /// </summary>
    public class PageContentService : Service, IPageContentService
    {
        /// <summary>
        /// The options service.
        /// </summary>
        private readonly IPageContentOptionsService optionsService;

        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The root option service
        /// </summary>
        private readonly Module.Root.Services.IOptionService rootOptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageContentService" /> class.
        /// </summary>
        /// <param name="optionsService">The options service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="rootOptionService">The root option service.</param>
        public PageContentService(IPageContentOptionsService optionsService, 
            IRepository repository, 
            IUnitOfWork unitOfWork,
            Module.Root.Services.IOptionService rootOptionService)
        {
            this.optionsService = optionsService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.rootOptionService = rootOptionService;
        }

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        IPageContentOptionsService IPageContentService.Options
        {
            get
            {
                return optionsService;
            }
        }

        /// <summary>
        /// Gets the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetPageContentResponse</c> with a page content.</returns>
        public GetPageContentResponse Get(GetPageContentRequest request)
        {
            var model =
                repository.AsQueryable<PageContent>(content => content.Id == request.PageContentId && content.Page.Id == request.PageId)
                    .Select(
                        pageContent =>
                        new PageContentModel
                            {
                                Id = pageContent.Id,
                                Version = pageContent.Version,
                                CreatedOn = pageContent.CreatedOn,
                                CreatedBy = pageContent.CreatedByUser,
                                LastModifiedOn = pageContent.ModifiedOn,
                                LastModifiedBy = pageContent.ModifiedByUser,
                                ContentId = pageContent.Content.Id,
                                PageId = pageContent.Page.Id,
                                RegionId = pageContent.Region.Id,
                                Order = pageContent.Order,
                                ParentPageContentId = pageContent.Parent != null ? pageContent.Parent.Id : (Guid?) null
                            })
                    .FirstOne();

            var response = new GetPageContentResponse { Data = model };

            if (request.Data.IncludeOptions)
            {
                response.Options = PageContentOptionsHelper.GetPageContentOptionsList(repository, request.PageContentId, rootOptionService);
            }

            return response;
        }

        public PostPageContentResponse Post(PostPageContentRequest request)
        {
            var result = Put(new PutPageContentRequest
                {
                    Data = request.Data,
                    User = request.User,
                    PageId = request.PageId
                });

            return new PostPageContentResponse { Data = result.Data };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageContentResponse</c> with saved page content id.</returns>
        public PutPageContentResponse Put(PutPageContentRequest request)
        {
            var isNew = !request.Id.HasValue || request.Id.Value.HasDefaultValue();
            PageContent pageContent = null;
            var isSorting = false;

            if (!isNew)
            {
                pageContent = repository
                    .AsQueryable<PageContent>()
                    .FirstOrDefault(content => content.Id == request.Id && content.Page.Id == request.PageId);
                isNew = pageContent == null;
            }

            if (isNew)
            {
                pageContent = new PageContent
                              {
                                  Id = request.Id.HasValue ? request.Id.Value : Guid.Empty,
                                  Page = repository.AsProxy<Module.Root.Models.Page>(request.PageId)
                              };
            }
            else if (request.Data.Version > 0)
            {
                isSorting = request.Data.RegionId != pageContent.Region.Id || request.Data.Order != pageContent.Order;
                pageContent.Version = request.Data.Version;
            }

            pageContent.Content = repository.AsProxy<Module.Root.Models.Content>(request.Data.ContentId);
            pageContent.Region = repository.AsProxy<Region>(request.Data.RegionId);
            pageContent.Order = request.Data.Order;

            if (request.Data.ParentPageContentId.HasValue && !request.Data.ParentPageContentId.Value.HasDefaultValue())
            {
                pageContent.Parent = repository.AsProxy<PageContent>(request.Data.ParentPageContentId.Value);
            }
            else
            {
                pageContent.Parent = null;
            }

            if (request.Data.Options != null)
            {
                var options = request.Data.Options.ToServiceModel();

                var contentOptions = pageContent.Options != null ? pageContent.Options.Distinct() : null;
                pageContent.Options = rootOptionService.SaveOptionValues(options, contentOptions, () => new PageContentOption
                                                                                                        {
                                                                                                            PageContent = pageContent
                                                                                                        });
            }

            unitOfWork.BeginTransaction();
            repository.Save(pageContent);
            unitOfWork.Commit();

            if (isNew)
            {
                Events.PageEvents.Instance.OnPageContentInserted(pageContent);
            }
            else if (isSorting)
            {
                Events.PageEvents.Instance.OnPageContentSorted(pageContent);
            }

            if (request.Data.Options != null)
            {
                Events.PageEvents.Instance.OnPageContentConfigured(pageContent);
            }

            return new PutPageContentResponse { Data = pageContent.Id };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageContentResponse</c> with success status.</returns>
        public DeletePageContentResponse Delete(DeletePageContentRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeletePageContentResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<PageContent>()
                .Where(p => p.Id == request.Id && p.Page.Id == request.PageId)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            repository.Delete(itemToDelete);
            unitOfWork.Commit();

            Events.PageEvents.Instance.OnPageContentDeleted(itemToDelete);

            return new DeletePageContentResponse { Data = true };
        }
    }
}