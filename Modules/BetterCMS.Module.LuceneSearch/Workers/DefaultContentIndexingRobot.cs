using System;
using System.Collections.Generic;
using System.Linq;
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
        public DefaultContentIndexingRobot(TimeSpan timespan)
            : base(timespan)
        {
        }

        protected override void DoWork()
        {
            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {
                var indexerService = lifetimeScope.Resolve<IIndexerService>();
                var scrapeService = lifetimeScope.Resolve<IScrapeService>();
                var crawlerService = lifetimeScope.Resolve<IWebCrawlerService>();

                string message;
                if (!crawlerService.IsConfigured(out message))
                {
                    Log.ErrorFormat("Cannot start Lucene web crawler: {0}", message);
                    return;
                }

                var links = scrapeService.GetLinksForProcessing();

                var pages = new List<PageData>();
                var idsToDelete = new List<Guid>();

                foreach (var link in links)
                {
                    scrapeService.MarkStarted(link.Id);

                    var response = crawlerService.FetchPage(link.Path);
                    response.IsPublished = link.IsPublished;
                    response.Id = link.Id;

                    switch (response.StatusCode)
                    {
                        case (HttpStatusCode.OK):
                            {
                                pages.Add(response);
                                break;
                            }

                        case HttpStatusCode.NotFound:
                            {
                                idsToDelete.Add(link.Id);
                                scrapeService.Delete(link.Id);
                                break;
                            }

                        default:
                            {
                                scrapeService.MarkFailed(link.Id);
                                // +2 hours (2, 4, 6, 8, 12)

//                                bool success = false;
//
//                                for (int i = 0; !success && i < RetryCount; i++)
//                                {
//                                    response = crawlerService.FetchPage(link.Path);
//                                    if (response.StatusCode == HttpStatusCode.OK)
//                                    {
//                                        success = true;
//                                    }
//                                }
//
//                                if (success)
//                                {
//                                    pages.Add(response);
//                                }
//                                else
//                                {
//                                    idsToDelete.Add(link.Id);
//                                    scrapeService.Delete(link.Id);
//                                }
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
                if (idsToDelete.Any())
                {
                    indexerService.DeleteDocuments(idsToDelete.Distinct().ToArray());
                }
                indexerService.Close();
            }

        }
    }
}