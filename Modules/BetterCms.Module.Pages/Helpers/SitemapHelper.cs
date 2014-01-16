using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for sitemap related tasks.
    /// </summary>
    public static class SitemapHelper
    {
        public class PageData
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public Guid? LanguageId { get; set; }
            public Guid? LanguageGroupIdentifier { get; set; }
        }

        public static IEnumerable<PageData> GetPagesToFuture(bool enableMultilanguage, IRepository repository)
        {
            return enableMultilanguage ? repository
                .AsQueryable<Root.Models.Page>()
                .Select(p => new PageData
                {
                    Id = p.Id,
                    Title = p.Title,
                    Url = p.PageUrl,
                    LanguageId = p.Language != null ? p.Language.Id : Guid.Empty,
                    LanguageGroupIdentifier = p.LanguageGroupIdentifier
                })
                .ToFuture() : null;
        }

        /// <summary>
        /// Gets the sitemap nodes in hierarchy.
        /// </summary>
        /// <param name="enableMultilanguage">if set to <c>true</c> multi-language is enabled.</param>
        /// <param name="sitemapNodes">The sitemap nodes.</param>
        /// <param name="allNodes">All nodes.</param>
        /// <param name="languageIds">The languages.</param>
        /// <param name="pages">The pages.</param>
        /// <returns>
        /// The list with all root nodes.
        /// </returns>
        public static List<SitemapNodeViewModel> GetSitemapNodesInHierarchy(
            bool enableMultilanguage, IList<SitemapNode> sitemapNodes, IList<SitemapNode> allNodes, List<Guid> languageIds, List<PageData> pages)
        {
            var nodeList = new List<SitemapNodeViewModel>();
            foreach (var node in sitemapNodes.OrderBy(node => node.DisplayOrder))
            {
                var pageLinked = node.Page != null;
                var nodeViewModel = new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version,
                    Title = pageLinked && node.UsePageTitleAsNodeTitle ? node.Page.Title : node.Title,
                    UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                    Url = pageLinked ? node.Page.PageUrl : node.Url,
                    PageId = pageLinked ? node.Page.Id : Guid.Empty,
                    DisplayOrder = node.DisplayOrder,
                    ChildNodes = GetSitemapNodesInHierarchy(enableMultilanguage, allNodes.Where(f => f.ParentNode == node).ToList(), allNodes, languageIds, pages)
                };

                if (enableMultilanguage)
                {
                    nodeViewModel.Translations = new List<SitemapNodeTranslationViewModel>();

                    node.Translations.Distinct()
                        .Select(t => new SitemapNodeTranslationViewModel
                        {
                            Id = t.Id,
                            LanguageId = t.Language.Id,
                            Title = t.Title,
                            UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
                            Url = t.Url,
                            Version = t.Version
                        })
                        .ToList()
                        .ForEach(nodeViewModel.Translations.Add);
                    
                    if (pageLinked)
                    {
                        // Setup default language.
                        if (node.Page.Language == null)
                        {
                            nodeViewModel.Url = node.Page.PageUrl;
                            if (nodeViewModel.UsePageTitleAsNodeTitle)
                            {
                                nodeViewModel.Title = node.Page.Title;
                            }
                        }
                        else if (node.Page.LanguageGroupIdentifier.HasValue)
                        {
                            var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                            && p.LanguageGroupIdentifier.Value == node.Page.LanguageGroupIdentifier.Value
                                                                            && (!p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()));
                            if (pageTranslation != null)
                            {
                                nodeViewModel.Url = pageTranslation.Url;
                                if (nodeViewModel.UsePageTitleAsNodeTitle)
                                {
                                    nodeViewModel.Title = pageTranslation.Title;
                                }
                            }
                        }

                        // Setup other languages.
                        foreach (var languageId in languageIds)
                        {
                            var translationViewModel = nodeViewModel.Translations.FirstOrDefault(t => t.LanguageId == languageId);
                            if (translationViewModel == null)
                            {
                                translationViewModel = new SitemapNodeTranslationViewModel
                                    {
                                        Id = Guid.Empty,
                                        LanguageId = languageId,
                                        Title = string.Empty,
                                        Url = string.Empty,
                                        UsePageTitleAsNodeTitle = true,
                                        Version = 0
                                    };
                                nodeViewModel.Translations.Add(translationViewModel);
                            }

                            var title = string.Empty;
                            var url = string.Empty;

                            if (node.Page.Language != null && node.Page.Language.Id == languageId)
                            {
                                title = node.Page.Title;
                                url = node.Page.PageUrl;
                            }
                            else if (node.Page.LanguageGroupIdentifier.HasValue)
                            {
                                var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                                && p.LanguageGroupIdentifier.Value == node.Page.LanguageGroupIdentifier.Value
                                                                                && p.LanguageId.HasValue
                                                                                && p.LanguageId.Value == languageId);
                                if (pageTranslation != null)
                                {
                                    title = pageTranslation.Title;
                                    url = pageTranslation.Url;
                                }
                            }

                            translationViewModel.Url = url;
                            if (translationViewModel.UsePageTitleAsNodeTitle)
                            {
                                translationViewModel.Title = title;
                            }

                            if (string.IsNullOrEmpty(translationViewModel.Title) || string.IsNullOrEmpty(translationViewModel.Url))
                            {
                                translationViewModel.Title = nodeViewModel.Title;
                                translationViewModel.Url = nodeViewModel.Url;
                            }

                        }
                    }
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList;
        }
    }
}