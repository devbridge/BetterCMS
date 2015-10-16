using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Category;

namespace BetterCms.Module.Root.Helpers
{
    public class CategoriesHelper
    {
        public static List<CategoryTreeNodeViewModel> GetCategoryTreeNodesInHierarchy(
            bool enableMultilanguage, IList<Category> sitemapNodes, IList<Category> allNodes, List<Guid> languageIds)
        {
            var nodeList = new List<CategoryTreeNodeViewModel>();
            foreach (var node in sitemapNodes.OrderBy(node => node.DisplayOrder))
            {
                var nodeViewModel = new CategoryTreeNodeViewModel
                {
                    Id = node.Id,
                    Version = node.Version,
                    Title = node.Name,
                    DisplayOrder = node.DisplayOrder,
                    ChildNodes = GetCategoryTreeNodesInHierarchy(enableMultilanguage, allNodes.Where(f => f.ParentCategory == node).ToList(), allNodes, languageIds),
                    Macro = node.Macro
                };

//                if (enableMultilanguage)
//                {
//                    nodeViewModel.Translations = new List<SitemapNodeTranslationViewModel>();
//
//                    node.Translations.Distinct()
//                        .Select(t => new SitemapNodeTranslationViewModel
//                        {
//                            Id = t.Id,
//                            LanguageId = t.Language.Id,
//                            Title = t.Title,
//                            UsePageTitleAsNodeTitle = t.UsePageTitleAsNodeTitle,
//                            Url = t.Url,
//                            Version = t.Version,
//                            Macro = t.Macro
//                        })
//                        .ToList()
//                        .ForEach(nodeViewModel.Translations.Add);
//
//                    if (linkedPage != null)
//                    {
//                        // Setup default language.
//                        if (!linkedPage.LanguageId.HasValue)
//                        {
//                            nodeViewModel.Url = linkedPage.Url;
//                            if (nodeViewModel.UsePageTitleAsNodeTitle)
//                            {
//                                nodeViewModel.Title = linkedPage.Title;
//                            }
//                        }
//                        else if (linkedPage.LanguageGroupIdentifier.HasValue)
//                        {
//                            // If non-default translation is added to sitemap, retrieving default page from page translations list
//                            var defaultPageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
//                                                                            && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
//                                                                            && (!p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()));
//                            if (defaultPageTranslation != null)
//                            {
//                                nodeViewModel.Url = defaultPageTranslation.Url;
//                                nodeViewModel.DefaultPageId = defaultPageTranslation.Id;
//                                if (nodeViewModel.UsePageTitleAsNodeTitle)
//                                {
//                                    nodeViewModel.Title = defaultPageTranslation.Title;
//                                }
//                            }
//                        }
//
//                        // Setup other languages.
//                        foreach (var languageId in languageIds)
//                        {
//                            var translationViewModel = nodeViewModel.Translations.FirstOrDefault(t => t.LanguageId == languageId);
//                            if (translationViewModel == null)
//                            {
//                                translationViewModel = new SitemapNodeTranslationViewModel
//                                {
//                                    Id = Guid.Empty,
//                                    LanguageId = languageId,
//                                    Title = linkedPage.Title,
//                                    Url = nodeViewModel.Url,
//                                    UsePageTitleAsNodeTitle = true,
//                                    Version = 0,
//                                    Macro = string.Empty
//                                };
//                                nodeViewModel.Translations.Add(translationViewModel);
//                            }
//
//                            var title = translationViewModel.Title;
//                            var url = translationViewModel.Url ?? nodeViewModel.Url;
//
//                            if (linkedPage.LanguageId != null && linkedPage.LanguageId == languageId)
//                            {
//                                title = linkedPage.Title;
//                                url = linkedPage.Url;
//                                translationViewModel.PageId = linkedPage.Id;
//                            }
//                            else if (linkedPage.LanguageGroupIdentifier.HasValue)
//                            {
//                                // Get page translation. If not exists, retrieve default language's translation
//                                var pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
//                                                                                && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
//                                                                                && p.LanguageId.HasValue
//                                                                                && p.LanguageId.Value == languageId);
//                                // If page has translation, set it's id
//                                if (pageTranslation != null)
//                                {
//                                    translationViewModel.PageId = pageTranslation.Id;
//                                }
//
//                                // If page translation does not exist, retrieve default language's translation
//                                if (pageTranslation == null)
//                                {
//                                    pageTranslation = pages.FirstOrDefault(p => p.LanguageGroupIdentifier.HasValue
//                                                                                && p.LanguageGroupIdentifier.Value == linkedPage.LanguageGroupIdentifier.Value
//                                                                                && (!p.LanguageId.HasValue || p.LanguageId.Value.HasDefaultValue()));
//                                }
//
//                                if (pageTranslation != null)
//                                {
//                                    title = pageTranslation.Title;
//                                    url = pageTranslation.Url;
//                                }
//                            }
//
//                            translationViewModel.Url = url;
//                            if (translationViewModel.UsePageTitleAsNodeTitle)
//                            {
//                                translationViewModel.Title = title;
//                            }
//                        }
//                    }
//                }

                nodeList.Add(nodeViewModel);
            }

            return nodeList;
        }
    }
}