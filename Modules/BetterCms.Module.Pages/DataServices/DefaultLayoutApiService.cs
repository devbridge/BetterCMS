using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataServices;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultLayoutApiService : ILayoutApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultLayoutApiService(IRepository repository)
        {
            this.repository = repository;
        }

//        public System.Collections.Generic.IList<Core.DataContracts.ILayout> GetLayouts()
//        {
//            return repository
//               .AsQueryable<Layout>()
//               .Fetch(l => l.LayoutRegions)
//               .Cast<Core.DataContracts.ILayout>()
//               .ToList();
//        }
    }
}