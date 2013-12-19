using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultCultureService : ICultureService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCultureService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultCultureService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of cultures.
        /// </summary>
        /// <returns>
        /// List of culture lookup values.
        /// </returns>
        public IEnumerable<LookupKeyValue> GetCultures()
        {
            return repository
                .AsQueryable<Culture>()
                .OrderBy(c => c.Code)
                .Select(c => new LookupKeyValue
                                 {
                                     Key = c.Id.ToString().ToLowerInvariant(),
                                     Value = c.Name
                                 })
                .ToFuture();
        }
    }
}