using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemap
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapCommand : CommandBase, ICommand<string, SearchableSitemapViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Sitemap root nodes.</returns>
        public SearchableSitemapViewModel Execute(string request)
        {
            var nodes = GetNodes(request);

            return new SearchableSitemapViewModel
                {
                    SearchQuery = request,
                    RootNodes = nodes
                };
        }

        public IList<SitemapNodeViewModel> GetNodes(string search)
        {
            var allNodes = Repository.AsQueryable<SitemapNode>().ToList();
            var allModels = GetSitemapNodesInHierarchy(allNodes.Where(f => f.ParentNode == null).ToList(), allNodes);

            if (string.IsNullOrEmpty(search))
            {
                return allModels;
            }

            // Remove branches without search results
            RemoveInvalidItems(allModels, search);

            return allModels;
        }

        private void RemoveInvalidItems(IList<SitemapNodeViewModel> models, string search)
        {
            var itemsToRemove = new List<SitemapNodeViewModel>();

            foreach (var model in models)
            {
                if (!AddToSearchResults(model, search))
                {
                    itemsToRemove.Add(model);
                }
                else
                {
                    RemoveInvalidItems(model.ChildNodes, search);
                }
            }

            foreach (var item in itemsToRemove)
            {
                models.Remove(item);
            }
        }

        private bool AddToSearchResults(SitemapNodeViewModel model, string search)
        {
            if (ContainsSearchQuery(model.Title, search) || ContainsSearchQuery(model.Url, search))
            {
                return true;
            }

            foreach (var childModel in model.ChildNodes)
            {
                var result = AddToSearchResults(childModel, search);
                if (result)
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsSearchQuery(string value, string search)
        {
            return value.ToLower().Contains(search.ToLower());
        }

        private List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes)
        {
            var nodeList = new List<SitemapNodeViewModel>();

            foreach (var node in sitemapNodes)
            {
                nodeList.Add(new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version,
                    Title = node.Title,
                    Url = node.Url,
                    DisplayOrder = node.DisplayOrder,
                    ChildNodes = GetSitemapNodesInHierarchy(allNodes.Where(f => f.ParentNode == node).ToList(), allNodes)
                });
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }
    }
}