// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultScrapeService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms;
using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using BetterCMS.Module.LuceneSearch.Models;

using BetterCms.Module.Root.Models;

namespace BetterCMS.Module.LuceneSearch.Services.ScrapeService
{
    public class DefaultScrapeService : IScrapeService
    {
        private readonly IRepository Repository;

        private IUnitOfWork UnitOfWork { get; set; }

        private readonly int scrapeLimit;
        
        private readonly bool scrapePrivatePages;

        private readonly TimeSpan pageExpireTimeout;

        public DefaultScrapeService(IRepository repository, IUnitOfWork unitOfWork, ICmsConfiguration cmsConfiguration)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;

            if (!int.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneMaxPagesPerQuery), out scrapeLimit)
                || scrapeLimit < 0)
            {
                scrapeLimit = 1000;
            }

            if (!TimeSpan.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LucenePageExpireTimeout), out pageExpireTimeout))
            {
                pageExpireTimeout = TimeSpan.FromMinutes(10);
            }

            bool.TryParse(cmsConfiguration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexPrivatePages), out scrapePrivatePages);
        }

        public void FetchNewUrls()
        {
            var query = Repository.AsQueryable<Page>()
                          .Where(page => !page.IsDeleted && !page.IsMasterPage)
                          .Where(page => !Repository.AsQueryable<IndexSource>().Any(crawlUrl => crawlUrl.SourceId == page.Id));

            if (!scrapePrivatePages)
            {
                query = query.Where(page => page.Status == PageStatus.Published);
            }

            var sourcesTosave = query
                          .Select(page =>
                                    new IndexSource
                                    {
                                        Path = page.PageUrl,
                                        SourceId = page.Id,
                                        IsPublished = page.Status == PageStatus.Published
                                    })
                          .ToList();

            var eventArgs = BetterCms.Events.LuceneEvents.Instance.OnFetchingNewUrls();

            if (eventArgs != null && eventArgs.IndexSources != null && eventArgs.IndexSources.Count > 0)
            {
                var indexSourceComparer = new BetterCMS.Module.LuceneSearch.Models.IndexSource.IndexSourceComparerByIdAndPath();
                var registeredSources = Repository.AsQueryable<IndexSource>().ToList();
                var additionalSources = eventArgs.IndexSources.Where(additionalSource => !registeredSources.Any(registeredSource => indexSourceComparer.Equals(additionalSource, registeredSource)))
                                                              .Where(additionalSource => !sourcesTosave.Any(sourceToSave => indexSourceComparer.Equals(additionalSource, sourceToSave)))
                                                              .Distinct(indexSourceComparer);

                sourcesTosave.AddRange(additionalSources);
            }
            
            // Change publish status where status has changed
            Repository.AsQueryable<IndexSource>()
                .Where(source => Repository.AsQueryable<Page>().Any(p => p.Id == source.SourceId &&
                                                ((source.IsPublished && p.Status != PageStatus.Published) || (!source.IsPublished && p.Status == PageStatus.Published))))
                .ToList()
                .ForEach(source =>
                             {
                                 source.IsPublished = !source.IsPublished;
                                 sourcesTosave.Add(source);
                             });

            UnitOfWork.BeginTransaction();

            int i = 0;
            foreach (var source in sourcesTosave)
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

        public Queue<CrawlLink> GetLinksForProcessing(int? limit = null)
        {
            if (!limit.HasValue)
            {
                limit = scrapeLimit;
            }

            var queue = new Queue<CrawlLink>();

            var unprocessedLinks = GetUnprocessedLinks(limit.Value);
            var processed = unprocessedLinks.Count;
            AddLinksToQueue(queue, unprocessedLinks);

            if (processed < limit)
            {
                var expiredLinks = GetExpiredLinks(limit.Value - processed);
                processed += expiredLinks.Count;
                AddLinksToQueue(queue, expiredLinks);
            }

            if (processed < limit)
            {
                var failedLinks = GetFailedLinks(limit.Value - processed);
                AddLinksToQueue(queue, failedLinks);
            }

            return queue;
        }

        private void AddLinksToQueue(Queue<CrawlLink> queue, IList<IndexSource> links)
        {
            foreach (var url in links)
            {
                queue.Enqueue(new CrawlLink { Id = url.Id, Path = url.Path, IsPublished = url.IsPublished });
            }
        }

        private IList<IndexSource> GetUnprocessedLinks(int limit)
        {
            IndexSource indexSourceAlias = null;

            var unprocessedUrls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.StartTime == null)
                          .OrderByAlias(() => indexSourceAlias.Id)
                          .Asc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            return unprocessedUrls;
        }

        private IList<IndexSource> GetExpiredLinks(int limit)
        {
            IndexSource indexSourceAlias = null;
            var endDate =  DateTime.Now.Subtract(pageExpireTimeout);

            var expiredUrls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.EndTime <= endDate && indexSourceAlias.FailedCount == 0)
                          .OrderByAlias(() => indexSourceAlias.EndTime)
                          .Desc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            return expiredUrls;
        }

        private IList<IndexSource> GetFailedLinks(int limit)
        {
            IndexSource indexSourceAlias = null;

            var urls =
                Repository.AsQueryOver(() => indexSourceAlias)
                          .Where(() => indexSourceAlias.FailedCount > 0 && indexSourceAlias.NextRetryTime <= DateTime.Now)
                          .OrderByAlias(() => indexSourceAlias.EndTime)
                          .Desc.Lock(() => indexSourceAlias)
                          .Read.Take(limit)
                          .List();

            return urls;
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
            url.NextRetryTime = null;
            url.FailedCount = 0;

            UnitOfWork.BeginTransaction();
            Repository.Save(url);
            UnitOfWork.Commit();
        }

        public void MarkFailed(Guid id)
        {
            var url = Repository.First<IndexSource>(x => x.Id == id);
            url.EndTime = DateTime.Now;
            url.FailedCount++;

            int addHours;
            switch (url.FailedCount)
            {
                case 1:
                    addHours = 1;
                    break;
                case 2:
                    addHours = 4;
                    break;
                case 3:
                    addHours = 6;
                    break;
                case 4:
                    addHours = 8;
                    break;
                default:
                    addHours = 12;
                    break;
            }
            url.NextRetryTime = DateTime.Now.AddHours(addHours);

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