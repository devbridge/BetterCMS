using System.Collections.Generic;
using System.Linq;

using BetterCms.Api;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Api.DataContracts;
using BetterCms.Module.Blog.Api.DataModels;
using BetterCms.Module.MediaManager.Api.DataModels;
using BetterCms.Module.Root.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Blog.Services
{
    public class DefaultAuthorService : IAuthorService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthorService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="repository">The repository.</param>
        public DefaultAuthorService(IUnitOfWork unitOfWork, IRepository repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of author lookup values.
        /// </summary>
        /// <returns>
        /// List of author lookup values.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<LookupKeyValue> GetAuthors()
        {
            Models.Author alias = null;
            LookupKeyValue lookupAlias = null;

            return unitOfWork.Session
                .QueryOver(() => alias)
                .SelectList(select => select
                    .Select(Projections.Cast(NHibernateUtil.String, Projections.Property<Models.Author>(c => c.Id))).WithAlias(() => lookupAlias.Key)
                    .Select(() => alias.Name).WithAlias(() => lookupAlias.Value)).Where(c => !c.IsDeleted)
                .OrderBy(o => o.Name).Asc()
                .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                .List<LookupKeyValue>();
        }

        public IQueryable<AuthorModel> GetAuthorsAsQueryable()
        {
            return
                repository.AsQueryable<Models.Author>()
                          .Select(
                              author =>
                              new AuthorModel
                                  {
                                      Id = author.Id,
                                      Version = author.Version,
                                      Name = author.Name,
                                      Image =
                                          author.Image == null
                                              ? null
                                              : new MediaImageModel
                                                  {
                                                      Id = author.Image.Id,
                                                      Version = author.Image.Version,
                                                      Caption = author.Image.Caption,
                                                      PublicUrl = author.Image.PublicUrl,
                                                      PublicThumbnailUrl = author.Image.PublicThumbnailUrl
                                                  }
                                  });
        }

        public AuthorCreateResponce CreateAuthor(AuthorCreateRequest request)
        {
            var author = new Models.Author();

            author.Name = request.Name;
            author.Image = request.ImageId.HasValue ? repository.AsProxy<MediaManager.Models.MediaImage>(request.ImageId.Value) : null;

            repository.Save(author);
            unitOfWork.Commit();

            BlogsApiContext.Events.OnAuthorCreated(author);

            return new AuthorCreateResponce { Author = GetAuthorsAsQueryable().First(model => model.Id == author.Id) };
        }

        public AuthorUpdateResponce UpdateAuthor(AuthorUpdateRequest request)
        {
            var author = repository.First<Models.Author>(request.AuthorId);

            author.Name = request.Name;
            author.Version = request.Version;
            author.Image = request.ImageId.HasValue ? repository.AsProxy<MediaManager.Models.MediaImage>(request.ImageId.Value) : null;

            repository.Save(author);
            unitOfWork.Commit();

            BlogsApiContext.Events.OnAuthorUpdated(author);

            return new AuthorUpdateResponce { Author = GetAuthorsAsQueryable().First(model => model.Id == request.AuthorId) };
        }

        public AuthorDeleteResponce DeleteAuthor(AuthorDeleteRequest request)
        {
            var author = repository.Delete<Models.Author>(request.AuthorId, request.Version);
            unitOfWork.Commit();

            BlogsApiContext.Events.OnAuthorDeleted(author);

            return new AuthorDeleteResponce { Deleted = GetAuthorsAsQueryable().Count(model => model.Id == author.Id) == 0 };
        }
    }
}