using System.Collections.Generic;
using System.Linq;

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
        /// <summary>
        /// The cache service
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSitemapService" /> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultSitemapService(ICacheService cacheService, IRepository repository, IUnitOfWork unitOfWork)
        {
            this.cacheService = cacheService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the root nodes.
        /// </summary>
        /// <param name="search">The search query.</param>
        /// <returns>
        /// Sitemap node list.
        /// </returns>
        public IList<SitemapNode> GetRootNodes(string search)
        {
            var queryableNodes = repository.AsQueryable<SitemapNode>();

            if (!string.IsNullOrEmpty(search))
            {
                queryableNodes = queryableNodes.Where(n => n.Title.ToLower().Contains(search.ToLower()) || n.Url.ToLower().Contains(search.ToLower()));
                var nodes = queryableNodes.ToList();
                var rootNodes = new List<SitemapNode>();

                foreach (var node in nodes)
                {
                    var rootNode = GetRootNode(node);
                    if (!rootNodes.Contains(rootNode))
                    {
                        rootNodes.Add(rootNode);
                    }
                }

                return nodes;
            }

            queryableNodes = queryableNodes.Where(n => n.ParentNode == null);
            return queryableNodes.ToList();
        }

        /// <summary>
        /// Gets the root node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>Node that has no parent.</returns>
        private SitemapNode GetRootNode(SitemapNode node)
        {
            if (node.ParentNode != null)
            {
                return GetRootNode(node.ParentNode);
            }

            return node;
        }
    }
}