using System;
using System.Collections.Generic;

using Autofac;

using BetterCMS.Module.LuceneSearch;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Core.Modules;
using BetterCMS.Module.LuceneSearch.Workers;

using BetterCms.Module.Search.Services;

using Common.Logging;

using HtmlAgilityPack;

namespace BetterCms.Module.LuceneSearch
{
    public class LuceneSearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "lucene";        

        private static List<IWorker> workers = new List<IWorker>();

        private static readonly ILog Log = LogManager.GetLogger(LuceneSearchConstants.LuceneSearchModuleLoggerNamespace);

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
                    TimeSpan indexerFrequency;
                    if (TimeSpan.TryParse(configuration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LuceneIndexerFrequency), out indexerFrequency))
                    {
                        if (indexerFrequency > TimeSpan.FromSeconds(0))
                        {
                            workers.Add(new DefaultContentIndexingRobot(indexerFrequency));
                        }
                    }

                    // New page URLs watcher
                    TimeSpan watcherFrequency;
                    if (TimeSpan.TryParse(configuration.Search.GetValue(LuceneSearchConstants.ConfigurationKeys.LucenePagesWatcherFrequency), out watcherFrequency))
                    {
                        if (watcherFrequency > TimeSpan.FromSeconds(0))
                        {
                            workers.Add(new DefaultIndexSourceWatcher(watcherFrequency));
                        }
                    }

                    workers.ForEach(f => f.Start());
                };
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().SingleInstance();
            containerBuilder.RegisterType<DefaultIndexerService>().As<ISearchService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawlerService>().InstancePerDependency();
        }
    }
}
