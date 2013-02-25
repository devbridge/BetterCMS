using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataServices;
using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.DataServices
{
    public class DefaultAuthorApiService : IAuthorApiService
    {
        private IRepository repository { get; set; }

        public DefaultAuthorApiService(IRepository repository)
        {
            this.repository = repository;
        }

        public System.Collections.Generic.IList<IAuthor> GetAuthors()
        {
            return repository
                .AsQueryable<Author>()
                .Cast<IAuthor>()
                .ToList();
        }
    }
}