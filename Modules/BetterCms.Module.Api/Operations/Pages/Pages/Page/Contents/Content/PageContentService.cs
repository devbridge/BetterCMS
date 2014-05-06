using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content.Options;

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
        /// Initializes a new instance of the <see cref="PageContentService" /> class.
        /// </summary>
        /// <param name="optionsService">The options service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public PageContentService(IPageContentOptionsService optionsService, IRepository repository, IUnitOfWork unitOfWork)
        {
            this.optionsService = optionsService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
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
                repository.AsQueryable<Module.Root.Models.PageContent>(content => content.Id == request.PageContentId)
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
                                Order = pageContent.Order
                            })
                    .FirstOne();
            return new GetPageContentResponse { Data = model };
        }

        /// <summary>
        /// Puts the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutPageContentResponse</c> with saved page content id.</returns>
        public PutPageContentResponse Put(PutPageContentRequest request)
        {
            var pageContent =
                repository.AsQueryable<Module.Root.Models.PageContent>()
                    .FirstOrDefault(content => content.Id == request.PageContentId);

            if (pageContent == null)
            {
                pageContent = new Module.Root.Models.PageContent
                                  {
                                      Id = request.PageContentId.HasValue ? request.PageContentId.Value : Guid.Empty,
                                  };
            }
            else
            {
                pageContent.Version = request.Data.Version;
            }

            pageContent.Page = repository.AsProxy<Module.Root.Models.Page>(request.Data.PageId);
            pageContent.Content = repository.AsProxy<Module.Root.Models.Content>(request.Data.ContentId);
            pageContent.Region = repository.AsProxy<Module.Root.Models.Region>(request.Data.RegionId);
            pageContent.Order = request.Data.Order;

            unitOfWork.BeginTransaction();
            repository.Save(pageContent);
            unitOfWork.Commit();

            return new PutPageContentResponse { Data = pageContent.Id };
        }

        /// <summary>
        /// Deletes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>DeletePageContentResponse</c> with success status.</returns>
        public DeletePageContentResponse Delete(DeletePageContentRequest request)
        {
            var pageContent = repository
                .AsQueryable<Module.Root.Models.PageContent>()
                .Where(p => p.Id == request.PageContentId)
                .FirstOne();

            repository.Delete(pageContent);
            unitOfWork.Commit();

            return new DeletePageContentResponse { Data = true };
        }
    }
}