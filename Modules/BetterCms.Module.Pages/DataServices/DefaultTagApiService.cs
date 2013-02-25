using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataServices;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultTagApiService : ITagApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultTagApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultTagApiService(IRepository repository)
        {
            this.repository = repository;
        }

        // Methods implementations
        public IList<ITag> GetTags()
        {
            return repository
                .AsQueryable<Tag>()
                .Cast<ITag>()
                .ToList();
        }
    }
}