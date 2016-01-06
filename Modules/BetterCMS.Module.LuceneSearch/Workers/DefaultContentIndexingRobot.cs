using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Autofac;

using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterModules.Core.Dependencies;

namespace BetterCMS.Module.LuceneSearch.Workers
{
    public class DefaultContentIndexingRobot : WorkerBase
    {
        private IIndexerService indexerService;

        public DefaultContentIndexingRobot(TimeSpan timespan)
            : base(timespan)
        {
        }

        public override void Start()
        {
            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {
                using (indexerService = lifetimeScope.Resolve<IIndexerService>())
                {
                    if (!indexerService.StartIndexer())
                    {
                        return;
                    }
                    indexerService.CleanLock();
                }
            }

            base.Start();
        }

        protected override void DoWork()
        {
            Log.Trace("Starting Lucene Content Indexing Robot.");

            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {
                using (indexerService = lifetimeScope.Resolve<IIndexerService>())
                {
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
                        if (hostShuttingDown)
                        {
                            return;
                        }

                        scrapeService.MarkStarted(link.Id);

                        PageData response;

                        try
                        {
                            response = crawlerService.FetchPage(link.Path);

                            if (hostShuttingDown)
                            {
                                return;
                            }
                        }
                        catch (Exception exc)
                        {
                            Log.Error("Unhandled exception occurred while fetching a page.", exc);
                            response = null;
                        }

                        if (response != null)
                        {
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

                                case HttpStatusCode.ServiceUnavailable:
                                    Log.Trace("Server Unavailable (503) - stop indexing for a moment.");
                                    scrapeService.MarkFailed(link.Id);
                                    return;

                                default:
                                {
                                    scrapeService.MarkFailed(link.Id);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            scrapeService.MarkFailed(link.Id);
                        }
                    }

                    if (hostShuttingDown)
                    {
                        return;
                    }

                    foreach (var page in pages)
                    {
                        indexerService.AddHtmlDocument(page);
                        scrapeService.MarkVisited(page.Id);

                        if (hostShuttingDown)
                        {
                            return;
                        }
                    }

                    if (idsToDelete.Any())
                    {
                        indexerService.DeleteDocuments(idsToDelete.Distinct().ToArray());
                    }

                    indexerService.OptimizeIndex();
                }
            }

            if (!hostShuttingDown)
            {
                Log.Trace("Lucene Content Indexing Robot finished indexing.");
            }
        }

        protected override void OnStop()
        {
            if (indexerService != null)
            {
                indexerService.Dispose();
            }
            base.OnStop();

            Log.Trace("Lucene Content Indexing Robot stopped by web server.");
        }
    }
}