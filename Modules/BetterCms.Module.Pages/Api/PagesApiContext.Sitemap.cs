using System;
using System.Collections.Generic;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;

using BetterCms.Core.Exceptions.Api;
using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public partial class PagesApiContext
    {
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
        /// <param name="request">The request.</param>
        /// <returns>
        /// Returns the list with sitemap nodes.
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<SitemapNode> GetNodes(GetNodesRequest request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetNodesRequest();
                }
                request.SetDefaultOrder(s => s.Title);

                return Repository.ToDataListResponse(request);
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