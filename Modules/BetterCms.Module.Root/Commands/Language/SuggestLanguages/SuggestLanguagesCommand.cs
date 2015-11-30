// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuggestLanguagesCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.SuggestLanguages
{
    /// <summary>
    /// A command to get languages list by filter.
    /// </summary>
    public class SuggestLanguagesCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of languages.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = model.Query.ToLowerInvariant();

            var alreadyAdded = Repository.AsQueryable<Models.Language>().Select(c => c.Code).ToList();
            alreadyAdded.Add(System.Globalization.CultureInfo.InvariantCulture.Name);

            return System.Globalization.CultureInfo
                .GetCultures(System.Globalization.CultureTypes.AllCultures)
                .Where(culture => culture.GetFullName().ToLowerInvariant().Contains(query))
                .Where(cullture => !alreadyAdded.Contains(cullture.Name))
                .OrderBy(culture => culture.Name)
                .Select(culture => new LookupKeyValue { Key = culture.Name, Value = culture.GetFullName() })
                .ToList();
        }
    }
}