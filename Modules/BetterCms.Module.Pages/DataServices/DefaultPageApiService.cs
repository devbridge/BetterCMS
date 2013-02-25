using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataServices;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultPageApiService : IPageApiService
    {
        private IRepository repository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPageApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultPageApiService(IRepository repository)
        {
            this.repository = repository;
        }

        public IList<IPage> GetPages()
        {
            return GetPagesQueryable().ToList();
        }

        public IQueryable<IPage> GetPagesQueryable()
        {
            return GetPagesQueryable<Page>();
        }

        public IQueryable<TEntity> GetPagesQueryable<TEntity>()
            where TEntity : Core.Models.Entity, IPage
        {
            return repository.AsQueryable<TEntity>();
        }
    }
}