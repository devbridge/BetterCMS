// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchController.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Search.Command.SearchQuery;
using BetterCms.Module.Search.Content.Resources;
using BetterCms.Module.Search.ViewModels;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Search.Controllers
{
    [ActionLinkArea(SearchModuleDescriptor.SearchAreaName)]
    public class SearchController : CmsControllerBase
    {
        public ActionResult Results(SearchRequestViewModel model)
        {
            SearchResultsViewModel results = null;
            if (ModelState.IsValid)
            {
                results = GetCommand<SearchQueryCommand>().ExecuteCommand(model);
            }
            else
            {
                var errorMessage = SearchGlobalization.SearchResults_FailedToGetResults;
                var modelError = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(modelError))
                {
                    errorMessage = string.Concat(errorMessage, " ", modelError);
                }

                Messages.AddError(errorMessage);
            }

            if (results == null && Messages.Error != null && Messages.Error.Count > 0)
            {
                results = new SearchResultsViewModel { ErrorMessage = Messages.Error[0] };
            }

            return PartialView("SearchResultsWidget", results);
        }
    }
}
