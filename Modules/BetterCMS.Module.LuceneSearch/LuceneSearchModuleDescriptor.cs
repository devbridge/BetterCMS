using System;
using System.Diagnostics;
using System.Threading;

using Autofac;

using BetterCMS.Module.LuceneSearch.Services;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.LuceneSearchService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;


namespace BetterCms.Module.LuceneSearch
{
    public class LuceneSearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "lucene";

        internal const string AreaName = "bcms-lucene";

        internal static Thread indexerWorker;

        internal static Thread urlWatcher;

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
                    // How to create long running thread
                    indexerWorker = new Thread(new ThreadStart(DoIndexWork));
                    indexerWorker.Start();
                    //TODO: add url parsing worker
                    //urlWatcher = new Thread(new ThreadStart(DoUrlWork));
                };                    
        }

        private void DoIndexWork()
        {
            while (true)
            {
                // How to resolve service
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var service = container.Resolve<ILuceneSearchService>();

                    service.Start();
                    // service1 == service2, nes InstancePerLifetimeScope
                    //service.UpdateIndex();
                }

                // TODO: next step - add sleep time to cms.config
                Thread.Sleep(5 * 60 * 1000);
            }
        }

        private void DoUrlWork()
        {
            while (true)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var service = container.Resolve<ILuceneSearchService>();

                    //service.ParseUrls();
                }
                Thread.Sleep(1 * 60 * 1000);
            }
        }

        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultLuceneSearchService>().As<ILuceneSearchService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawleService>().InstancePerDependency();
        }
    }
}
