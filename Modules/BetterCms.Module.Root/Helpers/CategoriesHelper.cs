// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoriesHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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