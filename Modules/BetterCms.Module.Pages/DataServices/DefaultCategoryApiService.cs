using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataServices;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultCategoryApiService : ICategoryApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultCategoryApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultCategoryApiService(IRepository repository)
        {
            this.repository = repository;
        }

        public System.Collections.Generic.IList<Core.DataContracts.ICategory> GetCategories()
        {
            return repository
                .AsQueryable<Category>()
                .Cast<Core.DataContracts.ICategory>()
                .ToList();
        }
    }
}