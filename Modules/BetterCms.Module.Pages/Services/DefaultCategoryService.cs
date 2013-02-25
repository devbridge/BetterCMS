using System.Collections.Generic;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Services
{
    internal class DefaultCategoryService : ICategoryService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectService" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultCategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>
        /// List of category lookup values
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<LookupKeyValue> GetCategories()
        {
            LookupKeyValue lookupAlias = null;
            Category alias = null;

            var query = unitOfWork
                .Session
                .QueryOver(() => alias)
                .Where(() => !alias.IsDeleted)
                .SelectList(select => select
                    .Select(NHibernate.Criterion.Projections.Cast(NHibernateUtil.String, NHibernate.Criterion.Projections.Property<Category>(c => c.Id))).WithAlias(() => lookupAlias.Key)
                    .Select(() => alias.Name).WithAlias(() => lookupAlias.Value))
                .TransformUsing(Transformers.AliasToBean<LookupKeyValue>());

                query.UnderlyingCriteria.AddOrder(new Order(NHibernate.Criterion.Projections.Property(() => alias.Name), true));

            return query.Future<LookupKeyValue>();
        }
    }
}