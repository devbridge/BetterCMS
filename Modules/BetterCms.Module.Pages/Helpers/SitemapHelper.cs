using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for sitemap related tasks.
    /// </summary>
    internal static class SitemapHelper
    {
        /// <summary>
        /// Gets the sitemap nodes in hierarchy.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="enableMultilanguage">if set to <c>true</c> multi-language is enabled.</param>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <param name="allNodes">All nodes.</param>
        /// <param name="languages">The languages.</param>
        /// <returns>
        /// The list with all root nodes.
        /// </returns>
        public static List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(IRepository repository, bool enableMultilanguage, IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes, List<Guid> languages)
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
                    ChildNodes = GetSitemapNodesInHierarchy(repository, enableMultilanguage, allNodes.Where(f => f.ParentNode == node).ToList(), allNodes, languages)
                };

                if (enableMultilanguage)
                {
                    nodeViewModel.Translations = node.Translations
                        .Distinct()
                        .Select(t => new SitemapNodeTranslationViewModel
                        {
                            Id = t.Id,
                            LanguageId = t.Language.Id,
                            Title = t.Title,
                            Url = GetUrl(repository, node, t),
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

                    foreach (var languageId in languages)
                    {
                        if (nodeViewModel.Translations.All(model => model.Id != languageId))
                        {
                            nodeViewModel.Translations.Add(new SitemapNodeTranslationViewModel
                            {
                                Id = Guid.Empty,
                                LanguageId = languageId,
                                Title = node.Title,
                                Url = GetUrl(repository, node, languageId),
                                Version = 1
                            });
                        }
                    }
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList.OrderBy(n => n.DisplayOrder).ToList();
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="node">The node.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>
        /// Language dependent URL.
        /// </returns>
        private static string GetUrl(IRepository repository, SitemapNode node, Guid languageId)
        {
            // Get default url.
            if (node.Page == null)
            {
                return node.Url;
            }

            if (node.Page.Language != null && node.Page.Language.Id == languageId)
            {
                return node.Page.PageUrl;
            }

            var pageWithLanguage =
                repository.AsQueryable<Root.Models.Page>()
                          .FirstOrDefault(
                              page => page.Language != null && page.Language.Id == languageId && page.LanguageGroupIdentifier == node.Page.LanguageGroupIdentifier);

            if (pageWithLanguage != null)
            {
                return pageWithLanguage.PageUrl;
            }

            return node.Page.PageUrl;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="node">The node.</param>
        /// <param name="translation">The translation.</param>
        /// <returns>
        /// Language dependent URL.
        /// </returns>
        private static string GetUrl(IRepository repository, SitemapNode node, SitemapNodeTranslation translation = null)
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
                    repository.AsQueryable<Root.Models.Page>()
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
                repository.AsQueryable<Root.Models.Page>().FirstOrDefault(page => page.Language != null
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