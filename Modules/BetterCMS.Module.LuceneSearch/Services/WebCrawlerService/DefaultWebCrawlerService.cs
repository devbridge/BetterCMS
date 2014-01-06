using System;
using System.IO;
using System.Net;
using System.Text;

using BetterCms;

using Common.Logging;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class DefaultWebCrawlerService : IWebCrawlerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly ICmsConfiguration cmsConfiguration;

        public DefaultWebCrawlerService(ICmsConfiguration cmsConfiguration)
        {
            this.cmsConfiguration = cmsConfiguration;
        }

        public PageData FetchPage(string url)
        {
            var response = new PageData();
            HttpWebResponse httpWebResponse = null;

            var webServer = cmsConfiguration.Search.GetValue(LuceneSearchConstants.WebSiteUrlConfigurationKey) ?? string.Empty;
            var fullUrl = string.Concat(webServer.TrimEnd('/'), "/", url.TrimStart('/'));
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(fullUrl);
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = 60 * 1000;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                response.StatusCode = httpWebResponse.StatusCode;
                response.AbsolutePath = httpWebResponse.ResponseUri.AbsolutePath;
                response.AbsoluteUri = httpWebResponse.ResponseUri.AbsoluteUri;

                using (Stream responseStream = httpWebResponse.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            response.Content = new HtmlDocument();
                            response.Content.LoadHtml(streamReader.ReadToEnd());
                        }
                    }
                }
            }
            catch (SystemException ex)
            {
                Log.ErrorFormat("Failed to fetch page by url {0}.", ex, url);

                if (ex.GetType() == typeof(WebException))
                {
                    var webException = (WebException)ex;
                    response.StatusCode = ((HttpWebResponse)webException.Response).StatusCode;
                }
            }
            finally
            {
                if (httpWebResponse != null)
                {                    
                    httpWebResponse.Close();
                }
            }

            return response;
        }
    }
}
