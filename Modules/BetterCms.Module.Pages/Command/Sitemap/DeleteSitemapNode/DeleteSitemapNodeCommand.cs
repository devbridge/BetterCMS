using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

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
            SitemapService.DeleteNode(request.Id, request.Version);
            return true;
        }
    }
}