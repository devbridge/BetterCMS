using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataServices;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultRedirectApiService : IRedirectApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultRedirectApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultRedirectApiService(IRepository repository)
        {
            this.repository = repository;
        }

//        public System.Collections.Generic.IList<IRedirect> GetRedirects()
//        {
//            return repository
//                .AsQueryable<Redirect>()
//                .Cast<IRedirect>()
//                .ToList();
//        }
    }
}