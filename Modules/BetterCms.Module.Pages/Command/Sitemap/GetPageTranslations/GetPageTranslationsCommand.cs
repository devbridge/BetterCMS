// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageTranslationsCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Sitemap.GetPageTranslations
{
    public class GetPageTranslationsCommand : CommandBase, ICommand<Guid, List<PageTranslationViewModel>>
    {
        public List<PageTranslationViewModel> Execute(Guid pageId)
        {
            if (!pageId.HasDefaultValue())
            {
                var page = Repository.FirstOrDefault<Root.Models.Page>(p => p.Id == pageId);
                if (page != null)
                {
                    if (page.LanguageGroupIdentifier == null)
                    {
                        return new List<PageTranslationViewModel>()
                            {
                                new PageTranslationViewModel
                                    {
                                        Id = page.Id,
                                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                                        Title = page.Title,
                                        PageUrl = page.PageUrl,
                                        PageUrlHash = page.PageUrlHash
                                    }
                            };
                    }

                    var translations =
                        Repository.AsQueryable<Root.Models.Page>().Where(p => p.LanguageGroupIdentifier == page.LanguageGroupIdentifier).Distinct().ToList();

                    return
                        translations.Select(
                            t =>
                            new PageTranslationViewModel
                                {
                                    Id = t.Id,
                                    LanguageId = t.Language != null ? t.Language.Id : (Guid?)null,
                                    Title = t.Title,
                                    PageUrl = t.PageUrl,
                                    PageUrlHash = t.PageUrlHash
                                }).ToList();
                }
            }

            return null;
        }
    }
}