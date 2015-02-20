using System;

using Autofac;

using BetterCms.Core.Modules;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Search
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class SearchModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "search";

        /// <summary>
        /// The search module area name.
        /// </summary>
        internal const string SearchAreaName = "bcms-search";
        
        /// <summary>
        /// The search module database schema name.
        /// </summary>
        internal const string SearchSchemaName = "bcms_search";

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleDescriptor" /> class.
        /// </summary>
        public SearchModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {            
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "A search module for Better CMS (has dependencies with the Google Site Search or the Lucene Search module).";
            }
        }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return SearchAreaName;
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
                return SearchSchemaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
        }        
    }
}
