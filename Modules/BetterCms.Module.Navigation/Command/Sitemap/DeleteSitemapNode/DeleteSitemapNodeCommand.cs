using System;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Navigation.Models;
using BetterCms.Module.Navigation.Services;
using BetterCms.Module.Navigation.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Navigation.Command.Sitemap.DeleteSitemapNode
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
            var node = Repository.AsProxy<SitemapNode>(request.Id);
            node.Version = request.Version;

            Repository.Delete(node);
            UnitOfWork.Commit();

            return true;
        }
    }
}