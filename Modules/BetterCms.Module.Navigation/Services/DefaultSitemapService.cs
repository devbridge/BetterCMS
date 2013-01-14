using System.Collections.Generic;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Services.Caching;
using BetterCms.Module.Navigation.Models;

namespace BetterCms.Module.Navigation.Services
{
    /// <summary>
    /// Default sitemap service.
    /// </summary>
    public class DefaultSitemapService : ISitemapService
    {
        private readonly ICacheService cacheService;
        private readonly IRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DefaultSitemapService(ICacheService cacheService, IRepository repository, IUnitOfWork unitOfWork)
        {
            this.cacheService = cacheService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public IList<SitemapNode> GetRootNodes(string search)
        {
            // TODO: implement.
            return new List<SitemapNode>();
        }
    }
}