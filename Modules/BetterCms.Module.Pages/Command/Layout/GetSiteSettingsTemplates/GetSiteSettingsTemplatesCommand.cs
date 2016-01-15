// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSiteSettingsTemplatesCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.ViewModels.SiteSettings;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Layout.GetSiteSettingsTemplates
{
    public class GetSiteSettingsTemplatesCommand : CommandBase, ICommand<SearchableGridOptions, SiteSettingTemplateListViewModel>
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="gridOptions">The request.</param>
        /// <returns>A list of paged\sorted widgets.</returns>
        public SiteSettingTemplateListViewModel Execute(SearchableGridOptions gridOptions)
        {
            var masterPages =
                Repository.AsQueryable<Root.Models.Page>()
                    .Where(f => f.IsDeleted == false && f.IsMasterPage)
                    .Select(
                        f =>
                        new SiteSettingTemplateItemViewModel
                        {
                            Id = f.Id,
                            Version = f.Version,
                            TemplateName = f.Title,
                            IsMasterPage = true,
                            Url = f.PageUrl
                        })
                    .ToList();

            var layouts =
               Repository.AsQueryable<Root.Models.Layout>()
                         .Where(f => f.IsDeleted == false)
                         .Select(
                             f =>
                             new SiteSettingTemplateItemViewModel
                             {
                                 Id = f.Id,
                                 Version = f.Version,
                                 TemplateName = f.Name,
                                 IsMasterPage = false
                             });

            var query = masterPages.Union(layouts).AsQueryable();

            if (gridOptions != null)
            {
                gridOptions.SetDefaultSortingOptions("TemplateName");
                if (!string.IsNullOrWhiteSpace(gridOptions.SearchQuery))
                {
                    var searchQuery = gridOptions.SearchQuery.ToLowerInvariant();
                    query = query.Where(f => f.TemplateName.ToLower().Contains(searchQuery));
                }
            }

            var count = query.Count();
            var templates = query.AddSortingAndPaging(gridOptions).ToList();

            return new SiteSettingTemplateListViewModel(templates, gridOptions, count);
        }
    }
}