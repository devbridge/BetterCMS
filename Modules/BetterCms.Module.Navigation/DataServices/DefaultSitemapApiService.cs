using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Navigation.Services;

namespace BetterCms.Module.Navigation.DataServices
{
    /// <summary>
    /// Navigation API service implementation.
    /// </summary>
    public class DefaultSitemapApiService : ApiServiceBase, ISitemapApiService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSitemapApiService"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="sitemapService">
        /// The sitemap Service.
        /// </param>
        public DefaultSitemapApiService(IRepository repository, ISitemapService sitemapService)
        {
            this.repository = repository;
            this.sitemapService = sitemapService;
        }

        /// <summary>
        /// Gets the sitemap tree.
        /// </summary>
        /// <returns>Returns list with root nodes.</returns>
        public IList<SitemapNode> GetSitemapTree()
        {
            try
            {
                return sitemapService.GetRootNodes(string.Empty);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap tree.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns> Returns sitemap node or exception <see cref="EntityNotFoundException" />.</returns>
        /// <exception cref="EntityNotFoundException">If node was not wound.</exception>
        public SitemapNode GetNode(Guid id)
        {
            try
            {
                return repository.First<SitemapNode>(id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap node by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Returns the list with sitemap nodes.</returns>
        public IList<SitemapNode> GetNodes(Expression<Func<SitemapNode, bool>> filter = null, Expression<Func<SitemapNode, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Title;
                }

                return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get sitemap nodes.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}