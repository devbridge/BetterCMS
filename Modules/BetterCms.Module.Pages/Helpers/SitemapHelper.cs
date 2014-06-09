using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Sitemap;
using BetterCms.Module.Root.Models;
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
                            var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                            && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
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
                                        Title = nodeViewModel.Title,
                                        Url = nodeViewModel.Url,
                                        UsePageTitleAsNodeTitle = true,
                                        Version = 0,
                                        Macro = string.Empty
                                    };
                                nodeViewModel.Translations.Add(translationViewModel);
                            }

                            var title = nodeViewModel.Title;
                            var url = nodeViewModel.Url;

                            if (linkedPage.LanguageId != null && linkedPage.LanguageId == languageId)
                            {
                                title = linkedPage.Title;
                                url = linkedPage.Url;
                            }
                            else if (linkedPage.LanguageGroupIdentifier.HasValue)
                            {
                                var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
                                                                                && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
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
                        }
                    }
                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList;
        }

        /// <summary>
        /// Determines whether [is page in sitemap] [the specified repository].
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="pageUrlHash">The page URL hash.</param>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="languageGroupIdentifier">The language group identifier.</param>
        /// <returns><c>true</c> if page is on sitemap; otherwise <c>false</c></returns>
        public static bool IsPageInSitemap(IRepository repository, string pageUrlHash,  Guid? pageId, Guid? languageGroupIdentifier)
        {
            if (languageGroupIdentifier.HasValue && languageGroupIdentifier.Value != default(Guid))
            {
                var pageIds =
                    repository.AsQueryable<Page>()
                        .Where(p => p.LanguageGroupIdentifier == languageGroupIdentifier && !p.IsDeleted)
                        .Select(p => p.Id);

                if (repository.AsQueryable<SitemapNode>().Any(n => !n.Sitemap.IsDeleted && !n.IsDeleted && n.Page != null && pageIds.Contains(n.Page.Id)))
                {
                    return true;
                }
            }

            var futureNodeExists =
                repository.AsQueryable<SitemapNode>()
                    .Where(
                        node =>
                        (node.UrlHash == pageUrlHash || node.Page.Id == pageId)
                        && !node.IsDeleted && !node.Sitemap.IsDeleted)
                    .Select(n => n.Id)
                    .ToFuture();

            var futureTranslationExists =
                repository.AsQueryable<SitemapNodeTranslation>()
                    .Where(
                        translation =>
                        (translation.UrlHash == pageUrlHash)
                        && !translation.IsDeleted && !translation.Node.IsDeleted && !translation.Node.Sitemap.IsDeleted)
                    .Select(n => n.Id)
                    .ToFuture();

            return futureNodeExists.ToList().Any() || futureTranslationExists.ToList().Any();
        }
    }
}