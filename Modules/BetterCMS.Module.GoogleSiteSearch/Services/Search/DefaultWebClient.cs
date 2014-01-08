using System.Net;
using System.Text;

namespace BetterCMS.Module.GoogleSiteSearch.Services.Search
{
    public class DefaultWebClient : IWebClient
    {
        public string DownloadData(string url)
        {
            using (var webClient = new WebClient())
            {                
                var data =  webClient.DownloadData(url);

                return Encoding.UTF8.GetString(data);
            }
        }
    }
}