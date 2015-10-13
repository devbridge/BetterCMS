using System;
using System.Collections.Generic;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models;
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

        public Author CreateAuthor(string name, Guid? imageId, string description)
        {
            var author = new Author();

            author.Name = name;
            author.Description = description;
            author.Image = imageId.HasValue ? repository.AsProxy<MediaManager.Models.MediaImage>(imageId.Value) : null;

            repository.Save(author);
            unitOfWork.Commit();

            // Notify.
            Events.BlogEvents.Instance.OnAuthorCreated(author);

            return author;
        }

        public Author UpdateAuthor(Guid authorId, int version, string name, Guid? imageId, string description)
        {
            var author = repository.First<Author>(authorId);

            author.Name = name;
            author.Description = description;
            author.Version = version;
            author.Image = imageId.HasValue ? repository.AsProxy<MediaManager.Models.MediaImage>(imageId.Value) : null;

            repository.Save(author);
            unitOfWork.Commit();

            // Notify.
            Events.BlogEvents.Instance.OnAuthorUpdated(author);

            return author;
        }
        
        public void DeleteAuthor(Guid authorId, int version)
        {
            var author = repository.Delete<Models.Author>(authorId, version);
            unitOfWork.Commit();

            // Notify.
            Events.BlogEvents.Instance.OnAuthorDeleted(author);
        }
    }
}