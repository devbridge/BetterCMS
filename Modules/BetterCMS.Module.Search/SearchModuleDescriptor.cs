using Autofac;

using BetterCms;
using BetterCms.Core.Modules;

namespace BetterCMS.Module.Search
{
    public class SearchModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "search";        

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
                return "A search module for Better CMS. It depends on a concrete search engine module (LuceneSearch or GoogleSiteSearch).";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleDescriptor" class./>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public SearchModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
        }
        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
        
        }
    }
}
