using System.Web;

using BetterCms;
using BetterCms.Module.Search.Models;
using BetterCms.Module.Search.Services;

using Newtonsoft.Json;

namespace BetterCMS.Module.GoogleSiteSearch.Services.Search
{
    public class DefaultSearchService : ISearchService
    {
        private readonly IWebClient webClient;

        private readonly ICmsConfiguration configuration;

        public DefaultSearchService(IWebClient webClient, ICmsConfiguration configuration)
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

            var url = string.Format("https://www.googleapis.com/customsearch/v1?key={0}&cx={1}&q={2}", 
                configuration.Search.GetValue("GoogleSiteSearchApiKey"), 
                configuration.Search.GetValue("GoogleSiteSearchEngineKey"), 
                HttpUtility.UrlEncode(request.Query));

            var data = webClient.DownloadData(url);

            var results = JsonConvert.DeserializeObject<SearchResults>(data);

            results.Query = request.Query;

            return results;
        }
    }
}