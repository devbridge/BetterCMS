using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Sitemap.GetSitemapVersion
{
    /// <summary>
    /// Command to get sitemap data.
    /// </summary>
    public class GetSitemapVersionCommand : CommandBase, ICommand<Guid, SitemapViewModel>
    {
        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Gets or sets the sitemap service.
        /// </summary>
        /// <value>
        /// The sitemap service.
        /// </value>
        public ISitemapService SitemapService { get; set; }

        /// <summary>
        /// Gets or sets the language service.
        /// </summary>
        /// <value>
        /// The language service.
        /// </value>
        public ILanguageService LanguageService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="versionId">The sitemap version identifier.</param>
        /// <returns>
        /// Sitemap view model.
        /// </returns>
        public SitemapViewModel Execute(Guid versionId)
        {
            var languagesFuture = CmsConfiguration.EnableMultilanguage ? LanguageService.GetLanguages() : null;

            // Return current or old version.
            var sitemap = Repository.AsQueryable<Models.Sitemap>()
                .Where(map => map.Id == versionId)
                .FetchMany(map => map.Nodes)
                .ThenFetch(node => node.Page)
                .FetchMany(map => map.Nodes)
                .ThenFetchMany(node => node.Translations)
                .Distinct()
                .ToList()
                .FirstOrDefault() ?? SitemapService.GetArchivedSitemapVersionForPreview(versionId);

            if (sitemap != null)
            {
                var model = new SitemapViewModel
                    {
                        Id = sitemap.Id,
                        Version = sitemap.Version,
                        Title = sitemap.Title,
                        RootNodes = GetSitemapNodesInHierarchy(sitemap.Nodes.Distinct().Where(f => f.ParentNode == null).ToList(), sitemap.Nodes.Distinct().ToList()),
                        AccessControlEnabled = CmsConfiguration.Security.AccessControlEnabled,
                        ShowLanguages = CmsConfiguration.EnableMultilanguage,
                        Languages = CmsConfiguration.EnableMultilanguage ? languagesFuture.ToList() : null,
                        IsReadOnly = true
                    };

                return model;
            }

            return null;
        }

        /// <summary>
        /// Gets the sitemap nodes in hierarchy.
        /// </summary>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <param name="allNodes">All nodes.</param>
        /// <returns>The list with all root nodes.</returns>
        private List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes)
        {
            var nodeList = new List<SitemapNodeViewModel>();

            foreach (var node in sitemapNodes)
            {
                var nodeViewModel = new SitemapNodeViewModel
                    {
                        Id = node.Id,
                        Version = node.Version,
                        Title = node.Title,
                        Url = node.Page != null ? node.Page.PageUrl : node.Url,
                        PageId = node.Page != null ? node.Page.Id : Guid.Empty,
                        DisplayOrder = node.DisplayOrder,
                        ChildNodes = GetSitemapNodesInHierarchy(allNodes.Where(f => f.ParentNode == node).ToList(), allNodes)
                    };

                if (CmsConfiguration.EnableMultilanguage)
                {
                    nodeViewModel.Translations = node.Translations
                        .Distinct()
                        .Select(t => new SitemapNodeTranslationViewModel
                        {
                            Id = t.Id,
                            LanguageId = t.Language.Id,
                            Title = t.Title,
                            Url = GetUrl(node, t),
                            Version = t.Version
                        })
                        .ToList();

                    if (node.Page != null && node.Page.Language != null && nodeViewModel.Translations.All(t => t.LanguageId != node.Page.Language.Id))
                    {
                        nodeViewModel.Translations.Add(new SitemapNodeTranslationViewModel
                        {
                            Id = Guid.Empty,
                            LanguageId = node.Page.Language.Id,
                            Title = node.Title,
                            Url = node.Page.PageUrl,
                            Version = 1
                        });
                    }
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }

        private string GetUrl(SitemapNode node, SitemapNodeTranslation translation = null)
        {
            if (translation == null)
            {
                // Get default url.
                if (node.Page == null)
                {
                    return node.Url;
                }

                if (node.Page.Language == null)
                {
                    return node.Page.PageUrl;
                }

                var pageWithoutLanguage =
                    Repository.AsQueryable<Root.Models.Page>()
                              .FirstOrDefault(page => page.Language == null && page.LanguageGroupIdentifier == node.Page.LanguageGroupIdentifier);

                if (pageWithoutLanguage != null)
                {
                    return pageWithoutLanguage.PageUrl;
                }

                return node.Page.PageUrl;
            }

            // Get url by language.
            if (node.Page == null)
            {
                return translation.Url;
            }

            if (node.Page.Language != null && translation.Language.Id == node.Page.Language.Id)
            {
                return node.Page.PageUrl;
            }

            var pageInLanguage =
                Repository.AsQueryable<Root.Models.Page>().FirstOrDefault(page => page.Language != null
                                                                                  && page.Language.Id == translation.Language.Id
                                                                                  && page.LanguageGroupIdentifier == node.Page.LanguageGroupIdentifier);

            if (pageInLanguage != null)
            {
                return pageInLanguage.PageUrl;
            }

            return node.Page.PageUrl;
        }
    }
}