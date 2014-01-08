using Autofac;

using BetterCMS.Module.GoogleSiteSearch.Services.Search;

using BetterCms;
using BetterCms.Core.Modules;
using BetterCms.Module.Search.Services;

namespace BetterCMS.Module.GoogleSiteSearch
{
    public class GoogleSiteSearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "googlesitesearch";        

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
                return "The Google Site Search integration module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleSiteSearchModuleDescriptor" class./>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public GoogleSiteSearchModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GoogleSiteSearchService>().As<ISearchService>().SingleInstance();
            containerBuilder.RegisterType<DefaultWebClient>().As<IWebClient>().SingleInstance();
        }
    }
}
