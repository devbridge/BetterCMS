using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterCMS.Module.LuceneSearch.Models;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Models;

namespace BetterCMS.Module.LuceneSearch.Services.ScrapeService
{
    public class DefaultScrapeService : IScrapeService
    {
        private readonly IRepository Repository;

        private IUnitOfWork UnitOfWork { get; set; }

        private const int ResultLimit = 10;

        // Timeouts in minutes

        private const int CrawlFrequency = 1;

        private const int EndTimeout = 5;

        public DefaultScrapeService(IRepository repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        public void FetchNewUrls()
        {
            var newSources = Repository.AsQueryable<Page>()
                          .Where(page => !page.IsDeleted && !page.IsMasterPage && page.Status == PageStatus.Published)
                          .Where(page => !Repository.AsQueryable<IndexSource>().Any(crawlUrl => crawlUrl.SourceId == page.Id))
                          .Select(page =>
                                    new IndexSource
                                    {
                                        Path = page.PageUrl,
                                        SourceId = page.Id                                       
                                    })
                          .ToList();
                          
            
            UnitOfWork.BeginTransaction();

            int i = 0;
            foreach (var source in newSources)
            {
                Repository.Save(source);

                // Flushes every 20-size batch.
                if (i++ % 20 == 0)
                {
                    UnitOfWork.Session.Flush();
                    UnitOfWork.Session.Clear();
                }
            }
            
            UnitOfWork.Commit();
        }

        public Queue<CrawlLink> GetUnprocessedLinks(int limit = 1000)
        {
            IndexSource indexSourceAlias = null;

            var unprocessedUrls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.EndTime == null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            var links = new Queue<CrawlLink>();

            foreach (var url in unprocessedUrls)
            {
                links.Enqueue(new CrawlLink { Id = url.Id, Path = url.Path });
            }

            var expiredUrls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.EndTime != null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            var expiredLinks = expiredUrls.Where(url => (DateTime.Now - url.EndTime).Value.TotalMinutes > CrawlFrequency).Take(ResultLimit);

            foreach (var link in expiredLinks)
            {
                links.Enqueue(new CrawlLink { Id = link.Id, Path = link.Path });
            }

            if (links.Count == 0)
            {
                links = GetFailedLinks();
            }

            return links;
        }
 
        public Queue<CrawlLink> GetFailedLinks(int limit = 1000)
        {
            IndexSource indexSourceAlias = null;

            var result = new Queue<CrawlLink>();

            var urls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.StartTime != null && indexSourceAlias.EndTime == null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            var failedLinks = urls.Where(url => (DateTime.Now - url.StartTime).Value.TotalMinutes > EndTimeout).Take(ResultLimit);

            foreach (var link in failedLinks)
            {
                result.Enqueue(new CrawlLink { Id = link.Id, Path = link.Path });
            }

            return result;
        }

        public Queue<CrawlLink> GetProcessedLinks(int limit = 1000)
        {
            IndexSource indexSourceAlias = null;

            var processedUrls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.StartTime != null && indexSourceAlias.EndTime != null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            var links = processedUrls.Where(url => (DateTime.Now - url.EndTime).Value.TotalMinutes < CrawlFrequency).Take(ResultLimit);

            var result = new Queue<CrawlLink>();

            foreach (var url in links)
            {
                result.Enqueue(new CrawlLink { Id = url.Id, Path = url.Path });
            }

            return result;
        }

        public void MarkStarted(Guid id)
        {
            var url = Repository.First<IndexSource>(x => x.Id == id);
            url.StartTime = DateTime.Now;

            UnitOfWork.BeginTransaction();
            Repository.Save(url);
            UnitOfWork.Commit();
        }

        public void MarkVisited(Guid id)
        {
            var url = Repository.First<IndexSource>(x => x.Id == id);
            url.EndTime = DateTime.Now;

            UnitOfWork.BeginTransaction();
            Repository.Save(url);
            UnitOfWork.Commit();
        }

        public void Delete(Guid id)
        {
            var url = Repository.First<IndexSource>(x => x.Id == id);

            UnitOfWork.BeginTransaction();
            Repository.Delete(url);
            UnitOfWork.Commit();
        }
    }
}