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
            Log.Trace("Starting Lucene Content Indexing Robot.");

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

                if (!indexerService.OpenWriter())
                {
                    Log.Error("Lucene Content Indexing Robot cannot continue. Failed to open writer.");
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
                                break;
                            }
                    }
                }

                foreach (var page in pages)
                {
                    indexerService.AddHtmlDocument(page);
                    scrapeService.MarkVisited(page.Id);
                }
                if (idsToDelete.Any())
                {
                    indexerService.DeleteDocuments(idsToDelete.Distinct().ToArray());
                }
                indexerService.CloseWriter();
            }

            Log.Trace("Lucene Content Indexing Robot finished indexing.");
        }
    }
}