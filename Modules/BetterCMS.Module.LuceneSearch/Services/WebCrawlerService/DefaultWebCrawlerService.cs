using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using BetterCms.Module.LuceneSearch;

using Common.Logging;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public class DefaultWebCrawlerService : IWebCrawlerService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();
        
        public PageData FetchPage(string url)
        {
            var response = new PageData();
            HttpWebResponse httpWebResponse = null;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(LuceneSearchModuleDescriptor.HostUrl + url);
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
