using System.Threading;

using Autofac;

using BetterCMS.Module.LuceneSearch.Services;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.LuceneSearchService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

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
                    urlWatcher = new Thread(new ThreadStart(DoUrlWork));
                    urlWatcher.Start();
                    indexerWorker = new Thread(new ThreadStart(DoIndexWork));
                    indexerWorker.Start();
                };                    
        }

        private void DoUrlWork()
        {
            while (true)
            {
                // How to resolve service
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var service = container.Resolve<ILuceneSearchService>();

                    service.Start();
                }

                // TODO: next step - add sleep time to cms.config
                Thread.Sleep(1 * 30 * 1000);
            }
        }

        private void DoIndexWork()
        {
            while (true)
            {
                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var service = container.Resolve<ILuceneSearchService>();
                    service.UpdateIndex();
                }
                Thread.Sleep(1 * 30 * 1000);
            }
        }

        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultLuceneSearchService>().As<ILuceneSearchService>().SingleInstance();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawlerService>().InstancePerDependency();
        }
    }
}
