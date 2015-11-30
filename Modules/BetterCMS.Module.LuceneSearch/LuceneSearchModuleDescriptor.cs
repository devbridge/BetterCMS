// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LuceneSearchModuleDescriptor.cs" company="Devbridge Group LLC">
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

                    Logger.InfoFormat("OnHostStart: started {0} Lucene Search workers.", workers.Count);
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
