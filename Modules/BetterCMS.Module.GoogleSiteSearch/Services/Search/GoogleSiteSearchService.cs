// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GoogleSiteSearchService.cs" company="Devbridge Group LLC">
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
using System.Web;

using BetterCms;
using BetterCms.Module.Search;
using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;

using Newtonsoft.Json;

namespace BetterCMS.Module.GoogleSiteSearch.Services.Search
{
    public class GoogleSiteSearchService : ISearchService
    {
        private const string GoogleSiteSearchUrl = "https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}&num={3}";

        private const string SkipParameterName = "start";

        private readonly IWebClient webClient;

        private readonly ICmsConfiguration configuration;

        public GoogleSiteSearchService(IWebClient webClient, ICmsConfiguration configuration)
        {
            this.configuration = configuration;
            this.webClient = webClient;
        }

        public SearchResults Search(SearchRequest request)
        {
            if (string.IsNullOrEmpty(request.Query))
            {
                return new SearchResults();
            }

            var take = request.Take > 0 ? request.Take : SearchModuleConstants.DefaultSearchResultsCount;

            var url = string.Format(GoogleSiteSearchUrl, 
                configuration.Search.GetValue("GoogleSiteSearchApiKey"), 
                configuration.Search.GetValue("GoogleSiteSearchEngineKey"), 
                HttpUtility.UrlEncode(request.Query),
                take);

            if (request.Skip > 0)
            {
                url = string.Format("{0}&{1}={2}", url, SkipParameterName, request.Skip);
            }

            var data = webClient.DownloadData(url);

            var results = JsonConvert.DeserializeObject<GoogleSearchResults>(data);

            results.Query = request.Query;
            results.TotalResults = results.SearchInformation.TotalResults;

            return results;
        }

        private class GoogleSearchResults : SearchResults
        {
            public GoogleSearchResults()
            {
                SearchInformation = new GoogleSearchResultsInformation();
            }

            public GoogleSearchResultsInformation SearchInformation { get; set; }
        }

        private class GoogleSearchResultsInformation
        {
            public int TotalResults { get; set; }
        }
    }
}