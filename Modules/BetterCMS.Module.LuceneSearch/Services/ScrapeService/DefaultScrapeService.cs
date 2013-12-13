using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BetterCMS.Module.LuceneSearch.Models;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using NHibernate.Criterion;

namespace BetterCMS.Module.LuceneSearch.Services.ScrapeService
{
    public class DefaultScrapeService : IScrapeService
    {
        private readonly IRepository Repository;

        private IUnitOfWork UnitOfWork { get; set; }

        // Timeouts in minutes
        //private const int CrawlFrequency = 3 * 60;

        public static int CrawlFrequency = 10;

        private const int EndTimeout = 5;

        public DefaultScrapeService(IRepository repository, IUnitOfWork unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
        }

        public void AddUniqueLink(IList<string> urls)
        {
            if (urls != null && urls.Count > 0)
            {
                var query = new StringBuilder("MERGE INTO [bcms_lucene].[IndexedPages] C USING ( VALUES");

                query.AppendFormat("('{0}',0,0)", urls[0]);

                for (int index = 1; index < urls.Count; index++)
                {
                    query.AppendFormat(",('{0}',0,0)", urls[index]);
                }

                query.AppendFormat(
                    ")T([Path], [StartTime], [EndTime]) ON C.[Path] = T.[Path] WHEN NOT MATCHED THEN INSERT ([Path], [StartTime], [EndTime], [Version], [CreatedByUser], [ModifiedByUser]) VALUES (T.[Path], CONVERT(datetime,'{0}'), NULL, 1, 'System', 'System');",
                    DateTime.Now);

                Repository.Execute(query.ToString());
            }
        }

        public Queue<CrawlLink> GetUnprocessedLinks(int limit = 1000)
        {
            CrawlUrl crawlUrlAlias = null;

            var unprocessedUrls =
                Repository.AsQueryOver(() => crawlUrlAlias)
                          .Where(() => crawlUrlAlias.EndTime == null)
                          .OrderByAlias(() => crawlUrlAlias.Id)
                          .Asc.Lock(() => crawlUrlAlias)
                          .Read.Take(limit)
                          .List();

            var links = new Queue<CrawlLink>();

            foreach (var url in unprocessedUrls)
            {
                links.Enqueue(new CrawlLink { Id = url.Id, Path = url.Path });
            }

            var expiredUrls =
                Repository.AsQueryOver(() => crawlUrlAlias)
                          .Where(() => crawlUrlAlias.EndTime != null)
                          .OrderByAlias(() => crawlUrlAlias.Id)
                          .Asc.Lock(() => crawlUrlAlias)
                          .Read.Take(limit)
                          .List();

            var expiredLinks = expiredUrls.Where(url => (DateTime.Now - url.EndTime).Value.TotalMinutes > CrawlFrequency);

            foreach (var link in expiredLinks)
            {
                links.Enqueue(new CrawlLink { Id = link.Id, Path = link.Path });
            }

            return links;
        }

        public Queue<CrawlLink> GetProcessedLinks(int limit = 1000)
        {
            CrawlUrl crawlUrlAlias = null;

            var processedUrls =
                Repository.AsQueryOver(() => crawlUrlAlias)
                          .Where(() => crawlUrlAlias.StartTime != null && crawlUrlAlias.EndTime != null)
                          .OrderByAlias(() => crawlUrlAlias.Id)
                          .Asc.Lock(() => crawlUrlAlias)
                          .Read.Take(limit)
                          .List();

            var links = processedUrls.Where(url => (DateTime.Now - url.EndTime).Value.TotalMinutes < CrawlFrequency);

            var result = new Queue<CrawlLink>();

            foreach (var url in links)
            {
                result.Enqueue(new CrawlLink { Id = url.Id, Path = url.Path });
            }

            return result;
        }

        public void MarkVisited(Guid id)
        {
            var url = Repository.First<CrawlUrl>(x => x.Id == id);
            url.EndTime = DateTime.Now;

            UnitOfWork.BeginTransaction();
            Repository.Save(url);
            UnitOfWork.Commit();
        }
    }
}