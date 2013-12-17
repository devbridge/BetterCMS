using System;
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

namespace BetterCms.Module.Pages.Command.Sitemap.SaveSitemap
{
    /// <summary>
    /// Saves sitemap data.
    /// </summary>
    public class SaveSitemapCommand : CommandBase, ICommand<SitemapViewModel, SitemapViewModel>
    {
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
        public SitemapViewModel Execute(SitemapViewModel request)
        {
            createdNodes.Clear();
            updatedNodes.Clear();
            deletedNodes.Clear();

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

                var roles = new[] { RootModuleConstants.UserRoles.EditContent };
                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite, roles);
                }
            }
            else
            {
                sitemap = new Models.Sitemap() { AccessRules = new List<AccessRule>() };
            }

            UnitOfWork.BeginTransaction();

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                sitemap.AccessRules.RemoveDuplicateEntities();

                var accessRules = request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().ToList() : null;
                AccessControlService.UpdateAccessControl(sitemap, accessRules);
            }

            sitemap.Title = request.Title;
            sitemap.Version = request.Version;
            Repository.Save(sitemap);

            SaveNodeList(sitemap, request.RootNodes, null);

            IList<Tag> newTags;
            TagService.SaveTags(sitemap, request.Tags, out newTags);


            UnitOfWork.Commit();

            if (createdNodes.Count <= 0 && updatedNodes.Count <= 0 && deletedNodes.Count <= 0)
            {
                return GetModelMainData(sitemap);
            }

            var updatedSitemaps = new List<Models.Sitemap>();
            foreach (var node in createdNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeCreated(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var node in updatedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeUpdated(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var node in deletedNodes)
            {
                Events.SitemapEvents.Instance.OnSitemapNodeDeleted(node);
                if (!updatedSitemaps.Contains(node.Sitemap))
                {
                    updatedSitemaps.Add(node.Sitemap);
                }
            }

            foreach (var updatedSitemap in updatedSitemaps)
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(updatedSitemap);
            }

            Events.RootEvents.Instance.OnTagCreated(newTags);

            return GetModelMainData(sitemap);
        }

        private SitemapViewModel GetModelMainData(Models.Sitemap sitemap)
        {
            return new SitemapViewModel
            {
                Id = sitemap.Id,
                Version = sitemap.Version,
                Title = sitemap.Title,
            };
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

                var sitemapNode = SitemapService.SaveNode(sitemap, node.Id, node.Version, node.Url, node.Title, node.DisplayOrder, node.ParentId, isDeleted, parentNode);

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