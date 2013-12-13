using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;

using HtmlAgilityPack;

namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    class DefaultWebCrawlerService : IWebCrawleService
    {
        private static readonly Regex RegexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))", RegexOptions.Compiled);
        private bool Succes = true;

        private static readonly string RootUrl = "http://bettercms.sandbox.mvc4.local";

        public CrawlerResult ProccessUrl(string url, Guid id)
        {
            return new CrawlerResult
            {
                NewUrls = Succes ? ParseLinks(url) : new List<string>(),
                CurrentUrl = url,
                Id = id,
                Succes = Succes
            };
        }

        public PageData FetchPage(string url)
        {
            var response = new PageData();
            HttpWebResponse httpWebResponse = null;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(RootUrl + url);
            httpWebRequest.AllowAutoRedirect = true;
            httpWebRequest.Timeout = 60 * 1000;

            try
            {
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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
                    else
                    {
                        Succes = false;
                    }
                }
            }
            catch (SystemException)
            {
                Succes = false;
            }

            if (httpWebResponse != null)
            {
                httpWebResponse.Close();
            }

            return response;
        }

        private IEnumerable<string> ParseLinks(string pageUrl)
        {
            pageUrl = pageUrl.Replace("http://bettercms.sandbox.mvc4.local", "");

            using (var api = ApiFactory.Create())
            {
                var request = new GetSitemapNodesRequest();
                var nodes = api.Pages.Sitemap.Nodes.Get(request);

                var rootNode = nodes.Data.Items.First(p => p.Url == pageUrl);

                var result = nodes.Data.Items.Where(n => n.ParentId == rootNode.Id).Select(n => n.Url);
                return result;
            }
        }

        public IList<string> GetRootNodes()
        {
            using (var api = ApiFactory.Create())
            {
                var request = new GetSitemapNodesRequest();
                var nodes = api.Pages.Sitemap.Nodes.Get(request);
                var rootNodes = nodes.Data.Items.Where(p => p.ParentId == null).Select(n => n.Url);
                return rootNodes.ToList();
            }
        } 

        public IList<string> GetPagesList()
        {
            using (var api = ApiFactory.Create())
            {
                var request = new GetPagesRequest();
                var items = api.Pages.Pages.Get(request);
                var result = items.Data.Items.Select(i => i.PageUrl);
                return result.ToList();
            }
        } 

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public int GetPageCount()
        {
            throw new NotImplementedException();
        }
    }
}
