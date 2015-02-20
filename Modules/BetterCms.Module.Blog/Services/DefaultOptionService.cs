using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Blog.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Blog.Services
{
    internal class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultOptionService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the default template id.
        /// </summary>
        /// <returns>
        /// Default template id or null, if such is not set
        /// </returns>
        public Option GetDefaultOption()
        {
            return repository.AsQueryable<Option>()
                .Fetch(option => option.DefaultLayout)

// In not supported by NHibernate - too deep.
//                .FetchMany(option => option.DefaultLayout.LayoutRegions)
//                .ThenFetch(region => region.Region)

                .Fetch(option => option.DefaultMasterPage)
                .ThenFetchMany(master => master.AccessRules)
                .Distinct()
                .ToList()
                .FirstOrDefault();
        }
    }
}