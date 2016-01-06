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

using BetterModules.Events;

using Common.Logging;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.LuceneSearch
{
    public class LuceneSearchModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        internal const string ModuleName = "lucene";        
        
        internal const string LuceneSchemaName = "bcms_lucene";        

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

        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return LuceneSchemaName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
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
            WebCoreEvents.Instance.HostStart += x =>
                {
                    Logger.Info("OnHostStart: preparing Lucene Search index workers...");

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

                    Logger.Info("OnHostStart: preparing Lucene Search index workers completed.");
                };
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultIndexerService>().As<ISearchService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawlerService>().InstancePerDependency();
        }
    }
}
