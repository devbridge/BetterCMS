using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Autofac;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Navigation.Services;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Navigation API context.
    /// </summary>
    public class NavigationApiContext : DataApiContext
    {
        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The container.</param>
        public NavigationApiContext(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
            sitemapService = Resolve<ISitemapService>();
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
                return Repository.First<SitemapNode>(id);
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

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
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