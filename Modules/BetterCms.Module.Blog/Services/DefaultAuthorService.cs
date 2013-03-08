using System.Collections.Generic;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Blog.Services
{
    internal class DefaultAuthorService : IAuthorService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAuthorService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultAuthorService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
            Author alias = null;
            LookupKeyValue lookupAlias = null;

            return unitOfWork.Session
                .QueryOver(() => alias)
                .SelectList(select => select
                    .Select(Projections.Cast(NHibernateUtil.String, Projections.Property<Author>(c => c.Id))).WithAlias(() => lookupAlias.Key)
                    .Select(() => alias.Name).WithAlias(() => lookupAlias.Value)).Where(c => !c.IsDeleted)
                .OrderBy(o => o.Name).Asc()
                .TransformUsing(Transformers.AliasToBean<LookupKeyValue>())
                .List<LookupKeyValue>();
        }
    }
}