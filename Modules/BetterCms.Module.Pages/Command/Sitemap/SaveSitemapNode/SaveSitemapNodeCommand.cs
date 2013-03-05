using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
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
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Execution result.</returns>
        public SitemapNodeViewModel Execute(SitemapNodeViewModel request)
        {
            var node = request.Id.HasDefaultValue()
                ? new SitemapNode()
                : Repository.AsProxy<SitemapNode>(request.Id);

            node.Version = request.Version;
            node.Title = request.Title;
            node.Url = request.Url;
            node.DisplayOrder = request.DisplayOrder;
            node.ParentNode = request.ParentId.HasDefaultValue()
                ? null
                : Repository.AsProxy<SitemapNode>(request.ParentId);

            Repository.Save(node);
            UnitOfWork.Commit();

            return new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version
                };
        }
    }
}