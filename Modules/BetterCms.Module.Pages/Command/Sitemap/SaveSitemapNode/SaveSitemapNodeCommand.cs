using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Sitemap.SaveSitemapNode
{
    /// <summary>
    /// Saves sitemap node data.
    /// </summary>
    public class SaveSitemapNodeCommand : CommandBase, ICommand<SitemapNodeViewModel, SitemapNodeViewModel>
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
        public SitemapNodeViewModel Execute(SitemapNodeViewModel request)
        {
            UnitOfWork.BeginTransaction();

            var node = SitemapService.SaveNode(request.Id, request.Version, request.Url, request.Title, request.DisplayOrder, request.ParentId);

            UnitOfWork.Commit();

            return new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version
                };
        }
    }
}