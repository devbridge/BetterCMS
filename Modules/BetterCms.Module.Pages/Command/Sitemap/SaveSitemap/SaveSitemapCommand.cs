using System.Collections.Generic;
using System.IO;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

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

                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    AccessControlService.DemandAccess(sitemap, Context.Principal, AccessLevel.ReadWrite);
                }
            }
            else
            {
                sitemap = new Models.Sitemap() { AccessRules = new List<AccessRule>() };
            }

            var nodeList = !createNew ? Repository.AsQueryable<SitemapNode>()
                                            .Where(node => node.Sitemap.Id == sitemap.Id)
                                            .ToFuture()
                                      : new List<SitemapNode>();
            var translationList = !createNew
                                      ? Repository.AsQueryable<SitemapNodeTranslation>()
                                            .Where(t => t.Node.Sitemap.Id == sitemap.Id)
                                            .Fetch(t => t.Node)
                                            .ToFuture()
                                      : new List<SitemapNodeTranslation>();

            UnitOfWork.BeginTransaction();

            if (!createNew)
            {
                SitemapService.ArchiveSitemap(request.Id);
            }

            if (CmsConfiguration.Security.AccessControlEnabled)
            {
                sitemap.AccessRules.RemoveDuplicateEntities();

                var accessRules = request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().ToList() : null;
                AccessControlService.UpdateAccessControl(sitemap, accessRules);
            }

            sitemap.Title = request.Title;
            sitemap.Version = request.Version;
            Repository.Save(sitemap);

            SaveNodeList(sitemap, request.RootNodes, null, nodeList.ToList(), translationList.ToList());

            IList<Tag> newTags;
            TagService.SaveTags(sitemap, request.Tags, out newTags);


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

            if (createNew)
            {
                Events.SitemapEvents.Instance.OnSitemapCreated(sitemap);
            }
            else
            {
                Events.SitemapEvents.Instance.OnSitemapUpdated(sitemap);
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
        /// <param name="sitemap">The sitemap.</param>
        /// <param name="nodes">The nodes.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="nodeList"></param>
        /// <param name="translationList"></param>
        private void SaveNodeList(Models.Sitemap sitemap, IEnumerable<SitemapNodeViewModel> nodes, SitemapNode parentNode, List<SitemapNode> nodeList, List<SitemapNodeTranslation> translationList)
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
                var translationUpdatedInDB = false;
                var sitemapNode = SitemapService.SaveNode(out updatedInDB, sitemap, node.Id, node.Version, node.Url, node.Title, node.Macro, node.PageId, node.UsePageTitleAsNodeTitle, node.DisplayOrder, node.ParentId, isDeleted, parentNode, nodeList);

                if ((create || update) && (node.Translations != null && node.Translations.Count > 0))
                {
                    SaveTranslations(out translationUpdatedInDB, sitemapNode, node, translationList);
                }

                if (create && (updatedInDB || translationUpdatedInDB))
                {
                    createdNodes.Add(sitemapNode);
                }
                else if (update && (updatedInDB || translationUpdatedInDB))
                {
                    updatedNodes.Add(sitemapNode);
                }
                else if (delete && updatedInDB)
                {
                    deletedNodes.Add(sitemapNode);
                    RemoveTranslations(sitemapNode);
                }

                SaveNodeList(sitemap, node.ChildNodes, sitemapNode, nodeList, translationList);
            }
        }

        /// <summary>
        /// Saves the translations.
        /// </summary>
        /// <param name="translationUpdatedInDb"></param>
        /// <param name="sitemapNode">The sitemap node.</param>
        /// <param name="node">The node.</param>
        /// <param name="translationList"></param>
        private void SaveTranslations(out bool translationUpdatedInDb, SitemapNode sitemapNode, SitemapNodeViewModel node, List<SitemapNodeTranslation> translationList)
        {
            translationUpdatedInDb = false;
            var translations = translationList == null
                                    ? Repository.AsQueryable<SitemapNodeTranslation>().Where(translation => translation.Node.Id == sitemapNode.Id).ToList()
                                    : translationList.Where(translation => translation.Node.Id == sitemapNode.Id).ToList();

            foreach (var model in node.Translations)
            {
                var saveIt = false;
                var translation = translations.FirstOrDefault(t => t.Id == model.Id);
                if (translation == null)
                {
                    translation = translations.FirstOrDefault(t => t.Language.Id == model.LanguageId);
                    if (translation != null)
                    {
                        throw new InvalidDataException(string.Format("Node {0} translation to language {1} already exists.", sitemapNode.Id, model.LanguageId));
                    }

                    saveIt = true;
                    translation = new SitemapNodeTranslation
                        {
                            Node = sitemapNode,
                            Language = Repository.AsProxy<Language>(model.LanguageId),
                            Title = model.Title,
                            Macro = CmsConfiguration.EnableMacros ? model.Macro : null,
                            UsePageTitleAsNodeTitle = model.UsePageTitleAsNodeTitle
                        };

                    if (sitemapNode.Page == null)
                    {
                        translation.UsePageTitleAsNodeTitle = false;
                        translation.Url = model.Url;
                        translation.UrlHash = model.Url.UrlHash();
                    }
                }
                else
                {
                    if (translation.Version != model.Version)
                    {
                        throw new ConcurrentDataException(translation);
                    }

                    if (translation.Title != model.Title)
                    {
                        saveIt = true;
                        translation.Title = model.Title;
                    }

                    if (CmsConfiguration.EnableMacros && translation.Macro != model.Macro)
                    {
                        saveIt = true;
                        translation.Macro = model.Macro;
                    }

                    if (sitemapNode.Page == null)
                    {
                        if (translation.Url != model.Url || translation.UsePageTitleAsNodeTitle)
                        {
                            saveIt = true;
                            translation.UsePageTitleAsNodeTitle = false;
                            translation.Url = model.Url;
                            translation.UrlHash = model.Url.UrlHash();
                        }
                    }
                    else
                    {
                        if (translation.Url != null || translation.UrlHash != null)
                        {
                            saveIt = true;
                            translation.Url = null;
                            translation.UrlHash = null;
                        }
                        if (translation.UsePageTitleAsNodeTitle != model.UsePageTitleAsNodeTitle)
                        {
                            saveIt = true;
                            translation.UsePageTitleAsNodeTitle = model.UsePageTitleAsNodeTitle;
                        }
                    }
                }

                if (saveIt)
                {
                    Repository.Save(translation);
                    translationUpdatedInDb = true;
                }
            }
        }

        /// <summary>
        /// Removes the translations.
        /// </summary>
        /// <param name="sitemapNode">The sitemap node.</param>
        private void RemoveTranslations(SitemapNode sitemapNode)
        {
            Repository.AsQueryable<SitemapNodeTranslation>()
                .Where(translation => translation.Node.Id == sitemapNode.Id)
                .ToList()
                .ForEach(translation => Repository.Delete(translation));
        }
    }
}