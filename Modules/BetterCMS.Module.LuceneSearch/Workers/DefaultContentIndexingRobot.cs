using System;
using System.Collections.Generic;
using System.Net;

using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Core.Dependencies;

using Autofac;

namespace BetterCMS.Module.LuceneSearch.Workers
{
    public class DefaultContentIndexingRobot : WorkerBase
    {
        private const int RetryCount = 10;

        public DefaultContentIndexingRobot()
            : base(new TimeSpan(0, 0, 10, 0))
        {
        }

        protected override void DoWork()
        {
            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {
                var indexerService = lifetimeScope.Resolve<IIndexerService>();
                var scrapeService = lifetimeScope.Resolve<IScrapeService>();
                var crawlerService = lifetimeScope.Resolve<IWebCrawlerService>();

                var links = scrapeService.GetUnprocessedLinks();

                var pages = new List<PageData>();

                foreach (var link in links)
                {
                    scrapeService.MarkStarted(link.Id);

                    var response = crawlerService.FetchPage(link.Path);
                    response.Id = link.Id;

                    switch (response.StatusCode)
                    {
                        case (HttpStatusCode.OK):
                            {
                                pages.Add(response);
                                break;
                            }
                            
                        case HttpStatusCode.InternalServerError:
                            {
                                bool success = false;

                                for (int i = 0; !success && i < RetryCount; i++)
                                {
                                    response = crawlerService.FetchPage(link.Path);
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        success = true;
                                    }
                                }

                                if (success)
                                {
                                    pages.Add(response);
                                }
                                else
                                {
                                    scrapeService.Delete(link.Id);
                                }
                                break;
                            }

                        case HttpStatusCode.NotFound:
                            {
                                scrapeService.Delete(link.Id);
                                break;
                            }
                    }
                }

                indexerService.Open();
                foreach (var page in pages)
                {
                    indexerService.AddHtmlDocument(page);
                    scrapeService.MarkVisited(page.Id);
                }
                indexerService.Close();
            }

        }
    }
}