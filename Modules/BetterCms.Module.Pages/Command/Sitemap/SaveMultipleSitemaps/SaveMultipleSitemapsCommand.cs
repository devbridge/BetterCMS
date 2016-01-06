using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.SaveMultipleSitemaps
{
    /// <summary>
    /// Saves sitemap data.
    /// </summary>
    public class SaveMultipleSitemapsCommand : CommandBase, ICommandIn<List<SitemapViewModel>>
    {
        /// <summary>
        /// Helper class 
        /// </summary>
        private class SaveModel
        {
            public SitemapViewModel Model { get; set; }
            public Models.Sitemap Entity { get; set; }
            public bool IsNew { get; set; }
        }

        private IList<SitemapNode> createdNodes = new List<SitemapNode>();
        private IList<SitemapNode> updatedNodes = new List<SitemapNode>();
        private IList<SitemapNode> deletedNodes = new List<SitemapNode>();

        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Gets or sets the tag service.
        /// </summary>
        /// <value>
        /// The tag service.
        /// </value>
        public ITagService TagService { get; set; }

        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Execute(List<SitemapViewModel> requests)
        {
            createdNodes.Clear();
            updatedNodes.Clear();
            deletedNodes.Clear();

            var sitemapsToSave = new List<SaveModel>();

            foreach (var request in requests)
            {
                var createNew = request.Id.HasDefaultValue();

                Models.Sitemap sitemap;

                if (!createNew)
                {
                    var sitemapQuery = Repository.AsQueryable<Models.Sitemap>()
                        .Where(s => s.Id == request.Id);

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        sitemapQuery = sitemapQuery.FetchMany(f => f.AccessRules);
                    }

                    sitemapQuery = sitemapQuery.FetchMany(s => s.Nodes);

                    sitemap = sitemapQuery.ToList().First(); // NOTE: bottleneck. TODO: remake with ToFuture to get all the sitemaps at once.

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite);
                    }
                }
                else
                {
                    sitemap = new Models.Sitemap() { AccessRules = new List<AccessRule>(), Nodes = new List<SitemapNode>() };
                }

                sitemapsToSave.Add(new SaveModel() { Model = request, Entity = sitemap, IsNew = createNew });
            }


            UnitOfWork.BeginTransaction();

            var createdTags = new List<Tag>();
            foreach (var item in sitemapsToSave)
            {
                // Save/update only sitemap nodes.
                SaveNodeList(item.Entity, item.Model.RootNodes, null, item.Entity.Nodes.ToList());
            }

            UnitOfWork.Commit();

            foreach (var node in createdNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
            }

            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
            }

            foreach (var item in sitemapsToSave)
            {
                if (item.IsNew)
                {
                    Events.SitemapEvents.Instance.OnSitemapCreated(item.Entity);
                }
                else
                {
                    Events.SitemapEvents.Instance.OnSitemapUpdated(item.Entity);
                }
            }

            Events.RootEvents.Instance.OnTagCreated(createdTags);
        }

        /// <summary>
        /// Saves the node list.
        /// </summary>
        /// <param name="sitemap"></param>
        /// <param name="nodes">The nodes.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="toList"></param>
        private void SaveNodeList(Models.Sitemap sitemap, IEnumerable<SitemapNodeViewModel> nodes, SitemapNode parentNode, List<SitemapNode> nodesToSearchIn)
        {
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes)
            {
                var isDeleted = node.IsDeleted || (parentNode != null && parentNode.IsDeleted);
                var create = node.Id.HasDefaultValue() && !isDeleted;
                var update = !node.Id.HasDefaultValue() && !isDeleted;
                var delete = !node.Id.HasDefaultValue() && isDeleted;

                bool updatedInDB;
                var sitemapNode = SaveNodeIfUpdated(out updatedInDB, sitemap, node.Id, node.Version, node.Url, node.Title, node.PageId, node.UsePageTitleAsNodeTitle, node.DisplayOrder, node.ParentId, isDeleted, parentNode, nodesToSearchIn);

                if (create && updatedInDB)
                {
                    createdNodes.Add(sitemapNode);
                }
                else if (update && updatedInDB)
                {
                    updatedNodes.Add(sitemapNode);
                }
                else if (delete && updatedInDB)
                {
                    deletedNodes.Add(sitemapNode);
                }

                SaveNodeList(sitemap, node.ChildNodes, sitemapNode, nodesToSearchIn);
            }
        }

        private SitemapNode SaveNodeIfUpdated(out bool nodeUpdated, Models.Sitemap sitemap, Guid nodeId, int version, string url, string title, Guid pageId, bool usePageTitleAsNodeTitle, int displayOrder, Guid parentId, bool isDeleted = false, SitemapNode parentNode = null, List<SitemapNode> nodesToSearchIn = null)
        {
            nodeUpdated = false;
            var node = nodeId.HasDefaultValue()
                ? new SitemapNode()
                : nodesToSearchIn != null ? nodesToSearchIn.First(n => n.Id == nodeId) : Repository.First<SitemapNode>(nodeId);

            if (isDeleted)
            {
                if (!node.Id.HasDefaultValue())
                {
                    Repository.Delete(node);
                    nodeUpdated = true;
                }
            }
            else
            {
                var updated = false;
                if (node.Sitemap == null)
                {
                    node.Sitemap = sitemap;
                }

                if (node.Title != title)
                {
                    updated = true;
                    node.Title = title;
                }

                if (node.Page != (!pageId.HasDefaultValue() ? Repository.AsProxy<PageProperties>(pageId) : null))
                {
                    updated = true;
                    node.Page = !pageId.HasDefaultValue() ? Repository.AsProxy<PageProperties>(pageId) : null;
                }

                if (node.UsePageTitleAsNodeTitle != (!pageId.HasDefaultValue() && usePageTitleAsNodeTitle))
                {
                    updated = true;
                    node.UsePageTitleAsNodeTitle = !pageId.HasDefaultValue() && usePageTitleAsNodeTitle;
                }

                if (node.Url != (node.Page != null ? null : url))
                {
                    updated = true;
                    node.Url = node.Page != null ? null : url;
                    node.UrlHash = node.Page != null ? null : url.UrlHash();
                }

                if (node.DisplayOrder != displayOrder)
                {
                    updated = true;
                    node.DisplayOrder = displayOrder;
                }

                SitemapNode newParent;
                if (parentNode != null && !parentNode.Id.HasDefaultValue())
                {
                    newParent = parentNode;
                }
                else
                {
                    newParent = parentId.HasDefaultValue()
                        ? null
                        : Repository.First<SitemapNode>(parentId);
                }

                if (node.ParentNode != newParent)
                {
                    updated = true;
                    node.ParentNode = newParent;
                }

                if (updated)
                {
                    node.Version = version;
                    Repository.Save(node);
                    nodeUpdated = true;
                }
            }

            return node;
        }
    }
}