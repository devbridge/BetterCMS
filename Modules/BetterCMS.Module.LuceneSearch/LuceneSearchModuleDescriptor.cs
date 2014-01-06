using System.Collections.Generic;

using Autofac;

using BetterCMS.Module.LuceneSearch;
using BetterCMS.Module.LuceneSearch.Services;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Core.Modules;
using BetterCMS.Module.LuceneSearch.Workers;

using BetterCms.Module.Search.Services;

namespace BetterCms.Module.LuceneSearch
{
    public class LuceneSearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "lucene";        

        private static List<IWorker> workers = new List<IWorker>();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        public override string Description
        {
            get
            {
                return "The Lucene Search module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuceneSearchModuleDescriptor" class./>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public LuceneSearchModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            Events.CoreEvents.Instance.HostStart += x =>
                {
                    // Content indexer
                    int minutes;
                    if (!int.TryParse(configuration.Search.GetValue(LuceneSearchConstants.ContentIndexerFrequencyConfigurationKey), out minutes))
                    {
                        minutes = 30;
                    }
                    workers.Add(new DefaultContentIndexingRobot(minutes));

                    // New page URLs watcher
                    if (!int.TryParse(configuration.Search.GetValue(LuceneSearchConstants.SourcePagesWatcherFrequencyConfigurationKey), out minutes))
                    {
                        minutes = 10;
                    }
                    workers.Add(new DefaultIndexSourceWatcher(minutes));

                    workers.ForEach(f => f.Start());
                };
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().SingleInstance();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawlerService>().InstancePerDependency();
            containerBuilder.RegisterType<LuceneSearchService>().As<ISearchService>().InstancePerDependency();
        }
    }
}
