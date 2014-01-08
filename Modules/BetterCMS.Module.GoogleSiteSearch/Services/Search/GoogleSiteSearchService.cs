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