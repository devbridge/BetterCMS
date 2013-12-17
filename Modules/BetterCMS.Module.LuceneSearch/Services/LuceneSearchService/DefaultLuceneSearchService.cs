using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BetterCMS.Module.LuceneSearch.Models;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

namespace BetterCMS.Module.LuceneSearch.Services.LuceneSearchService
{
    public class DefaultLuceneSearchService : ILuceneSearchService
    {
        private const int MaxTasksCount = 10;

        private Queue<CrawlLink> UrlQueue;

        private IList<string> urlList;

        private readonly IIndexerService indexerService;

        private readonly IScrapeService scrapeService;

        private readonly IWebCrawlerService webCrawlerService;

        public DefaultLuceneSearchService(IScrapeService scrapeService, IWebCrawlerService webCrawlerService, IIndexerService indexerService)
        {
            this.scrapeService = scrapeService;
            this.webCrawlerService = webCrawlerService;
            this.indexerService = indexerService;

            UrlQueue = new Queue<CrawlLink>();
        }

        public void Start()
        {
            urlList = webCrawlerService.GetPagesList();

            SaveUrls();
           
            UrlQueue = scrapeService.GetUnprocessedLinks();

            while (UrlQueue.Count > 0)
            {
                Crawl();
            }
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

            for (int i = 0; i < currentTasksCount; i++)
            {
                var result = tasks[i].Result;

                if (result.Success)
                {
                    scrapeService.MarkVisited(tasks[i].Result.Id);
                }
            }

            SaveUrls();

            scrapeService.AddUniqueLink(newUrls.ToList());
                
            if (UrlQueue.Count == 0)
            {
                UrlQueue = scrapeService.GetUnprocessedLinks();
            }
        }

        public void UpdateIndex()
        {
            var pages = scrapeService.GetProcessedLinks();

            foreach (var page in pages)
            {
                var result = webCrawlerService.FetchPage(page.Path);
                
                indexerService.AddHtmlDocument(result);
            }

            indexerService.Commit();
        }

        private void SaveUrls()
        {
            var urls = new List<string>();

            int limit = urlList.Count > MaxTasksCount ? MaxTasksCount : urlList.Count;
            
            for (int i = 0; i < limit; i++)
            {
                urls.Add(urlList[urlList.Count - 1]);
                urlList.RemoveAt(urlList.Count - 1);
            }

            scrapeService.AddUniqueLink(urls.ToList());
        }
    }
}