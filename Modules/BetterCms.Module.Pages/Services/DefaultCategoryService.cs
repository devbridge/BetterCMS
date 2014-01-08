using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Services
{
    internal class DefaultCategoryService : ICategoryService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultCategoryService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of category lookup values.
        /// </summary>
        /// <returns>
        /// List of category lookup values
        /// </returns>
        public IEnumerable<LookupKeyValue> GetCategories()
        {
            return repository
                .AsQueryable<Category>()
                .Select(c => new LookupKeyValue
                                 {
                                     Key = c.Id.ToString().ToLowerInvariant(),
                                     Value = c.Name
                                 })
                .ToFuture();
        }
    }
}