using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;

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
                    var sitemapQuery = Repository.AsQueryable<Models.Sitemap>().Where(s => s.Id == request.Id);

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        sitemapQuery = sitemapQuery.FetchMany(f => f.AccessRules);
                    }

                    sitemap = sitemapQuery.ToList().First();

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite);
                    }
                }
                else
                {
                    sitemap = new Models.Sitemap() { AccessRules = new List<AccessRule>() };
                }

                sitemapsToSave.Add(new SaveModel() { Model = request, Entity = sitemap, IsNew = createNew });
            }


            UnitOfWork.BeginTransaction();

            var createdTags = new List<Tag>();
            foreach (var item in sitemapsToSave)
            {
                IList<Tag> newTags;
                SaveIt(item.Entity, item.Model, out newTags);
                createdTags.AddRange(newTags);
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
        /// Saves it.
        /// </summary>
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="model">The model.</param>
        private void SaveIt(Models.Sitemap sitemap, SitemapViewModel model, out IList<Tag> newTags)
        {
            if (!sitemap.Id.HasDefaultValue())
            {
                SitemapService.ArchiveSitemap(sitemap.Id);
            }

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                sitemap.AccessRules.RemoveDuplicateEntities();

                var accessRules = model.UserAccessList != null ? model.UserAccessList.Cast<IAccessRule>().ToList() : null;
                AccessControlService.UpdateAccessControl(sitemap, accessRules);
            }

            sitemap.Title = model.Title;
            sitemap.Version = model.Version;
            Repository.Save(sitemap);

            SaveNodeList(sitemap, model.RootNodes, null);

            TagService.SaveTags(sitemap, model.Tags, out newTags);
        }

        /// <summary>
        /// Saves the node list.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="parentNode">The parent node.</param>
        private void SaveNodeList(Models.Sitemap sitemap, IEnumerable<SitemapNodeViewModel> nodes, SitemapNode parentNode)
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

                var sitemapNode = SitemapService.SaveNode(sitemap, node.Id, node.Version, node.Url, node.Title, node.PageId, node.UsePageTitleAsNodeTitle, node.DisplayOrder, node.ParentId, isDeleted, parentNode);

                if (create)
                {
                    createdNodes.Add(sitemapNode);
                }
                else if (update)
                {
                    updatedNodes.Add(sitemapNode);
                }
                else if (delete)
                {
                    deletedNodes.Add(sitemapNode);
                }

                SaveNodeList(sitemap, node.ChildNodes, sitemapNode);
            }
        }
   }
}