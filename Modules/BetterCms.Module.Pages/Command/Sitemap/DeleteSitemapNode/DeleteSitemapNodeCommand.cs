using System;
using System.Collections.Generic;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Sitemap.DeleteSitemapNode
{
    /// <summary>
    /// Delete sitemap node.
    /// </summary>
    public class DeleteSitemapNodeCommand : CommandBase, ICommand<SitemapNodeViewModel, bool>
    {
        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public bool Execute(SitemapNodeViewModel request)
        {
            UnitOfWork.BeginTransaction();

            IList<SitemapNode> deletedNodes;
            SitemapService.DeleteNode(request.Id, request.Version, out deletedNodes);

            UnitOfWork.Commit();

            var updatedSitemaps = new List<Models.Sitemap>();
            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var sitemap in updatedSitemaps)
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(sitemap);
            }

            return true;
        }
    }
}