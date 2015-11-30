// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SearchQueryCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Exceptions;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using BetterCms.Module.Search.Helpers;
using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;
using BetterCms.Module.Search.ViewModels;

using BetterModules.Core.Web.Mvc.Commands;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Search.Command.SearchQuery
{
    public class SearchQueryCommand : CommandBase, ICommand<SearchRequestViewModel, SearchResultsViewModel>
    {
        private readonly ISearchService searchService;

        private readonly IHttpContextAccessor httpContextAccessor;

        public SearchQueryCommand(ISearchService searchService, IHttpContextAccessor httpContextAccessor)
        {
            this.searchService = searchService;
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The request.</param>
        /// <returns></returns>
        public SearchResultsViewModel Execute(SearchRequestViewModel model)
        {
            var query = model.WidgetModel.GetSearchQueryParameter(httpContextAccessor.GetCurrent().Request, model.Query);
            SearchResults results;

            if (searchService == null)
            {
                throw new CmsException("The Better CMS Search Service is not found. Please install BetterCms.Module.GoogleSiteSearch or BetterCms.Module.LuceneSearch module.");
            }

            if (!string.IsNullOrWhiteSpace(query))
            {
                var take = model.WidgetModel.GetOptionValue<int>(SearchModuleConstants.WidgetOptionNames.ResultsCount);
                if (take <= 0)
                {
                    take = SearchModuleConstants.DefaultSearchResultsCount;
                }
                results = searchService.Search(new SearchRequest(query, take, model.Skip));
            }
            else
            {
                results = new SearchResults();
            }

            return new SearchResultsViewModel
                       {
                           Results = results,
                           WidgetViewModel = model.WidgetModel
                       };
        }
    }
}