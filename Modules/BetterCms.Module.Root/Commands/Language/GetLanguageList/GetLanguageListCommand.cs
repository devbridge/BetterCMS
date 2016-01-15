// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetLanguageListCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.Language;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.GetLanguageList
{
    public class GetLanguageListCommand : CommandBase, ICommand<SearchableGridOptions, SearchableGridViewModel<LanguageViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>List with languages view models</returns>
        public SearchableGridViewModel<LanguageViewModel> Execute(SearchableGridOptions request)
        {
            SearchableGridViewModel<LanguageViewModel> model;

            request.SetDefaultSortingOptions("Name");

            var query = Repository
                .AsQueryable<Models.Language>();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                query = query.Where(language => language.Name.Contains(request.SearchQuery) || language.Code.Contains(request.SearchQuery));
            }

            var languagesFuture = query
                .Select(language =>
                    new LanguageViewModel
                        {
                            Id = language.Id,
                            Version = language.Version,
                            Name = language.Name,
                            Code = language.Code
                        });

            var count = query.ToRowCountFutureValue();
            languagesFuture = languagesFuture.AddSortingAndPaging(request);

            var languages = languagesFuture.ToList();
            SetLanguageCodes(languages);
            model = new SearchableGridViewModel<LanguageViewModel>(languages, request, count.Value);

            return model;
        }

        private void SetLanguageCodes(List<LanguageViewModel> languages)
        {
            var codes = languages.Select(c => c.Code).ToArray();
            var names = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => codes.Contains(c.Name))
                .ToDictionary(c => c.Name, c => c.GetFullName());

            foreach (var pair in names)
            {
                languages.Where(c => c.Code == pair.Key).ToList().ForEach(c => c.Code = pair.Value);
            }
        }
    }
}