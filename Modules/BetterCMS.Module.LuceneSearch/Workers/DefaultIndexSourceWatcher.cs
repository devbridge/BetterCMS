using System;
using Autofac;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterModules.Core.Dependencies;

namespace BetterCMS.Module.LuceneSearch.Workers
{
    public class DefaultIndexSourceWatcher : WorkerBase
    {
        public DefaultIndexSourceWatcher(TimeSpan timespan)
            : base(timespan)
        {
        }

        protected override void DoWork()
        {
            Log.Trace("Starting Lucene Index Source Watcher.");

            using (var lifetimeScope = ContextScopeProvider.CreateChildContainer())
            {
                var scrapeService = lifetimeScope.Resolve<IScrapeService>();

                scrapeService.FetchNewUrls();
            }

            Log.Trace("Lucene Index Source Watcher finished looking for new sources.");
        }
    }
}
