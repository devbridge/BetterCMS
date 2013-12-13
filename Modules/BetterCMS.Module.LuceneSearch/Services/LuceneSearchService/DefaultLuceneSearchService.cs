using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BetterCMS.Module.LuceneSearch.Models;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

namespace BetterCMS.Module.LuceneSearch.Services.LuceneSearchService
{
    public class DefaultLuceneSearchService : ILuceneSearchService
    {
        private string RootUrl = "/";

        private const int MaxTasksCount = 10;

        private Queue<CrawlLink> UrlQueue;

        private readonly IIndexerService indexerService;

        private readonly IScrapeService scrapeService;

        private readonly IWebCrawleService webCrawlerService;

        public DefaultLuceneSearchService(IScrapeService scrapeService, IWebCrawleService webCrawlerService, IIndexerService indexerService)
        {
            this.scrapeService = scrapeService;
            this.webCrawlerService = webCrawlerService;
            this.indexerService = indexerService;

            UrlQueue = new Queue<CrawlLink>();
        }

        public void Start()
        {
            scrapeService.AddUniqueLink(webCrawlerService.GetRootNodes());
            UrlQueue = scrapeService.GetUnprocessedLinks();

            while (UrlQueue.Count > 0)
            {
                Crawl();
            }

            indexerService.Close();
        }


        private void Crawl()
        {
            int currentTasksCount = UrlQueue.Count > MaxTasksCount ? MaxTasksCount : UrlQueue.Count;
            var tasks = new Task<CrawlerResult>[currentTasksCount];

            for (int taskNo = 0; taskNo < currentTasksCount; taskNo++)
            {
                tasks[taskNo] = Task<CrawlerResult>.Factory.StartNew(
                    x =>
                        {
                            var link = (CrawlLink)x;

                            return webCrawlerService.ProccessUrl(link.Path, link.Id);
                        },
                    UrlQueue.Dequeue(),
                    TaskCreationOptions.LongRunning);
            }

            Task.WaitAll(tasks);

            var newUrls = new HashSet<string>();
            var failedUrls = new List<Guid>();

            for (int i = 0; i < currentTasksCount; i++)
            {
                var result = tasks[i].Result;

                if (result.Succes)
                {
                    foreach (var newUrl in result.NewUrls)
                    {
                        newUrls.Add(newUrl);
                    }
                    indexerService.AddHtmlDocument(result);
                    scrapeService.MarkVisited(tasks[i].Result.Id);
                }
                else
                {
                    failedUrls.Add(result.Id);
                }
            }

                scrapeService.AddUniqueLink(newUrls.ToList());
                
                if (UrlQueue.Count == 0)
                {
                    UrlQueue = scrapeService.GetUnprocessedLinks();
                }            
        }
    }
}