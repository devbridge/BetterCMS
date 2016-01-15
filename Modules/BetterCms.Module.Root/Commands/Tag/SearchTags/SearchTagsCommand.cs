// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchTagsCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Tag.SearchTags
{
    /// <summary>
    /// A command to get tag list by filter.
    /// </summary>
    public class SearchTagsCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of tags.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = Repository.AsQueryable<Models.Tag>()
                .Where(tag => tag.Name.Contains(model.Query));

            if (model.ExistingItemsArray.Length > 0)
            {
                query = query.Where(tag => !model.ExistingItems.Contains(tag.Name) && !model.ExistingItems.Contains(tag.Id.ToString().ToUpper()));
            }

            return query.OrderBy(tag => tag.Name)
                .Select(tag => new LookupKeyValue { Key = tag.Id.ToString(), Value = tag.Name })
                .ToList();
        }
    }
}