using System;
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;

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
            public bool IsPublished { get; set; }
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
                    LanguageGroupIdentifier = p.LanguageGroupIdentifier,
                    IsPublished = p.Status == PageStatus.Published
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
                var linkedPage = node.Page != null && !node.Page.IsDeleted
                                     ? pages.FirstOrDefault(p => p.Id == node.Page.Id)
                                       ?? new PageData
                                           {
                                               Id = node.Page.Id,
                                               Title = node.Page.Title,
                                               Url = node.Page.PageUrl,
                                               IsPublished = node.Page.Status == PageStatus.Published,
                                               LanguageId = node.Page.Language != null ? node.Page.Language.Id : (Guid?)null,
                                               LanguageGroupIdentifier = node.Page.LanguageGroupIdentifier
                                           }
                                     : null;

                var nodeViewModel = new SitemapNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version,
                    Title = linkedPage != null && node.UsePageTitleAsNodeTitle ? linkedPage.Title : node.Title,
                    UsePageTitleAsNodeTitle = node.UsePageTitleAsNodeTitle,
                    Url = linkedPage != null ? linkedPage.Url : node.Url,
                    PageId = linkedPage != null ? linkedPage.Id : Guid.Empty,
                    DefaultPageId = linkedPage != null ? linkedPage.Id : Guid.Empty,
                    PageTitle = linkedPage != null ? linkedPage.Title : null,
                    DisplayOrder = node.DisplayOrder,
                    ChildNodes = GetSitemapNodesInHierarchy(enableMultilanguage, allNodes.Where(f => f.ParentNode == node).ToList(), allNodes, languageIds, pages),
                    Macro = node.Macro
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
                                Version = t.Version,
                                Macro = t.Macro
                            })
                        .ToList()
                        .ForEach(nodeViewModel.Translations.Add);
                    
                    if (linkedPage != null)
                    {
                        // Setup default language.
                        if (!linkedPage.LanguageId.HasValue)
                        {
                            nodeViewModel.Url = linkedPage.Url;
                            if (nodeViewModel.UsePageTitleAsNodeTitle)
                            {
                                nodeViewModel.Title = linkedPage.Title;
                            }
                        }
                        else if (linkedPage.LanguageGroupIdentifier.HasValue)
                        {
                            // If non-default translation is added to sitemap, retrieving default page from page translations list
                            var defaultPageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                            && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
                                                                            && (!p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()));
                            if (defaultPageTranslation != null)
                            {
                                nodeViewModel.Url = defaultPageTranslation.Url;
                                nodeViewModel.DefaultPageId = defaultPageTranslation.Id;
                                if (nodeViewModel.UsePageTitleAsNodeTitle)
                                {
                                    nodeViewModel.Title = defaultPageTranslation.Title;
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
                                        Title = linkedPage.Title,
                                        Url = nodeViewModel.Url,
                                        UsePageTitleAsNodeTitle = true,
                                        Version = 0,
                                        Macro = string.Empty
                                    };
                                nodeViewModel.Translations.Add(translationViewModel);
                            }

                            var title = translationViewModel.Title;
                            var url = translationViewModel.Url ?? nodeViewModel.Url;

                            if (linkedPage.LanguageId != null && linkedPage.LanguageId == languageId)
                            {
                                title = linkedPage.Title;
                                url = linkedPage.Url;
                                translationViewModel.PageId = linkedPage.Id;
                            }
                            else if (linkedPage.LanguageGroupIdentifier.HasValue)
                            {
                                // Get page translation. If not exists, retrieve default language's translation
                                var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                                && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
                                                                                && p.LanguageId.HasValue
                                                                                && p.LanguageId.Value == languageId);
                                // If page has translation, set it's id
                                if (pageTranslation != null)
                                {
                                    translationViewModel.PageId = pageTranslation.Id;
                                }
                                
                                // If page translation does not exist, retrieve default language's translation
                                if (pageTranslation == null)
                                {
                                    pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                                && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
                                                                                && (!p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()));
                                }

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
                        }
                    }
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList;
        }
    }
}