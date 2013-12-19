using System.Collections.Generic;
using System.Threading;

using Autofac;

using BetterCMS.Module.LuceneSearch.Services;
using BetterCMS.Module.LuceneSearch.Services.IndexerService;
using BetterCMS.Module.LuceneSearch.Services.ScrapeService;
using BetterCMS.Module.LuceneSearch.Services.WebCrawlerService;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCMS.Module.LuceneSearch.Workers;


namespace BetterCms.Module.LuceneSearch
{
    public class LuceneSearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "lucene";        

        internal static readonly string HostUrl = "http://bettercms.sandbox.mvc4.local/";

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
                    workers.ForEach(f => f.Start());
                };
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            workers.Add(new DefaultContentIndexingRobot());
            workers.Add(new DefaultIndexSourceWatcher());

            containerBuilder.RegisterType<DefaultIndexerService>().As<IIndexerService>().SingleInstance();
            containerBuilder.RegisterType<DefaultScrapeService>().As<IScrapeService>().InstancePerDependency();
            containerBuilder.RegisterType<DefaultWebCrawlerService>().As<IWebCrawlerService>().InstancePerDependency();
        }
    }
}
