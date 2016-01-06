using System;
using System.Linq;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Exceptions.DataTier;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors.Author
{
    public class AuthorService : Service, IAuthorService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public AuthorService(IRepository repository, IMediaFileUrlResolver fileUrlResolver, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetAuthorRequest</c> with an author.
        /// </returns>
        public GetAuthorResponse Get(GetAuthorRequest request)
        {
            var query = repository.AsQueryable<Module.Blog.Models.Author>();
            
            query = query.Where(author => author.Id == request.AuthorId);
            
            var model = query
                .Select(author => new AuthorModel
                    {
                        Id = author.Id,
                        Version = author.Version,
                        CreatedBy = author.CreatedByUser,
                        CreatedOn = author.CreatedOn,
                        LastModifiedBy = author.ModifiedByUser,
                        LastModifiedOn = author.ModifiedOn,

                        Name = author.Name,
                        Description = author.Description,

                        ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                        ImageUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicUrl : (string)null,
                        ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicThumbnailUrl : (string)null,
                        ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null
                    })
                .FirstOne();

            model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
            model.ImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageThumbnailUrl);

            return new GetAuthorResponse
                       {
                           Data = model
                       };
        }

        /// <summary>
        /// Replaces the author or if it doesn't exist, creates it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PutAuthorResponse</c> with a author id.
        /// </returns>
        public PutAuthorResponse Put(PutAuthorRequest request)
        {
            var authors = repository.AsQueryable<Module.Blog.Models.Author>()
                .Where(l => l.Id == request.Id)
                .ToList();

            var authorToSave = authors.FirstOrDefault(l => l.Id == request.Id);

            var createAuthor = authorToSave == null;
            if (createAuthor)
            {
                authorToSave = new Module.Blog.Models.Author
                {
                    Id = request.Id.GetValueOrDefault()
                };
            }
            else if (request.Data.Version > 0)
            {
                authorToSave.Version = request.Data.Version;
            }

            unitOfWork.BeginTransaction();

            authorToSave.Name = request.Data.Name;
            authorToSave.Description = request.Data.Description;
            authorToSave.Image = request.Data.ImageId.HasValue ? repository.AsProxy<MediaImage>(request.Data.ImageId.Value) : null;

            repository.Save(authorToSave);

            unitOfWork.Commit();

            // Fire events.
            if (createAuthor)
            {
                Events.BlogEvents.Instance.OnAuthorCreated(authorToSave);
            }
            else
            {
                Events.BlogEvents.Instance.OnAuthorUpdated(authorToSave);
            }

            return new PutAuthorResponse
            {
                Data = authorToSave.Id
            };
        }

        /// <summary>
        /// Deletes the specified author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>DeleteAuthorResponse</c> with success status.
        /// </returns>
        public DeleteAuthorResponse Delete(DeleteAuthorRequest request)
        {
            if (request.Data == null || request.Id.HasDefaultValue())
            {
                return new DeleteAuthorResponse { Data = false };
            }

            var itemToDelete = repository
                .AsQueryable<Module.Blog.Models.Author>()
                .Where(p => p.Id == request.Id)
                .FirstOne();

            if (request.Data.Version > 0 && itemToDelete.Version != request.Data.Version)
            {
                throw new ConcurrentDataException(itemToDelete);
            }

            unitOfWork.BeginTransaction();

            repository.Delete(itemToDelete);

            unitOfWork.Commit();

            Events.BlogEvents.Instance.OnAuthorDeleted(itemToDelete);

            return new DeleteAuthorResponse { Data = true };
        }
    }
}