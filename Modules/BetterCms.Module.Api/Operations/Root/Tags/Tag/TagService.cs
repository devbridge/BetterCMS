using System.Linq;

using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    /// <summary>
    /// Default tag CRUD service.
    /// </summary>
    public class TagService : Service, ITagService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public TagService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the specified tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetTagResponse</c> with a tag.
        /// </returns>
        public GetTagResponse Get(GetTagRequest request)
        {
            var query = repository.AsQueryable<Module.Root.Models.Tag>();

            if (request.TagId.HasValue)
            {
                query = query.Where(tag => tag.Id == request.TagId);
            }
            else
            {
                query = query.Where(tag => tag.Name == request.TagName);
            }

            var model = query
                .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Version = tag.Version,
                        CreatedBy = tag.CreatedByUser,
                        CreatedOn = tag.CreatedOn,
                        LastModifiedBy = tag.ModifiedByUser,
                        LastModifiedOn = tag.ModifiedOn,

                        Name = tag.Name
                    })
                .FirstOne();

            return new GetTagResponse
                       {
                           Data = model
                       };
        }

        /// <summary>
        /// Replaces the tag or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutTagResponse</c> with a tag id.
        /// </returns>
        public PutTagResponse Put(PutTagRequest request)
        {
            var tagName = request.Data.Name.Trim();
            var tagsByIdFuture = repository.AsQueryable<Module.Root.Models.Tag>().Where(tag1 => tag1.Id == request.Id).ToFuture();
            var tagsByIdNameFuture = repository.AsQueryable<Module.Root.Models.Tag>().Where(tag1 => tag1.Name == tagName).ToFuture();
            var tagById = tagsByIdFuture.FirstOrDefault();
            var tagByName = tagsByIdNameFuture.FirstOrDefault();

            // Validate.
            if (tagById != null && tagByName != null && tagById.Id != tagByName.Id)
            {
                var logMessage = string.Format("Failed to rename tag. Tag with the same name already exists. Name: '{0}'.", tagName);
                throw new CmsApiValidationException(logMessage);
            }

            if (tagById == null && tagByName != null)
            {
                var logMessage = string.Format("Failed to create a tag. Tag with the same name already exists. Name: '{0}'.", tagName);
                throw new CmsApiValidationException(logMessage);
            }

            // Create or update.
            var createTag = tagById == null;
            if (createTag)
            {
                tagById = new Module.Root.Models.Tag { Id = request.Id.GetValueOrDefault() };
            }
            else if (request.Data.Version > 0)
            {
                tagById.Version = request.Data.Version;
            }

            tagById.Name = tagName;

            repository.Save(tagById);
            unitOfWork.Commit();

            // Fire events.
            if (createTag)
            {
                Events.RootEvents.Instance.OnTagCreated(tagById);
            }
            else
            {
                Events.RootEvents.Instance.OnTagUpdated(tagById);
            }

            return new PutTagResponse
            {
                Data = tagById.Id,
            };
        }

        /// <summary>
        /// Deletes the specified tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteTagResponse</c> with success status.
        /// </returns>
        public DeleteTagResponse Delete(DeleteTagRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteTagResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<Module.Root.Models.Tag>()
                .Where(p => p.Id == request.Id)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            repository.Delete(itemToDelete);

            unitOfWork.Commit();

            Events.RootEvents.Instance.OnTagDeleted(itemToDelete);

            return new DeleteTagResponse { Data = true };
        }
    }
}