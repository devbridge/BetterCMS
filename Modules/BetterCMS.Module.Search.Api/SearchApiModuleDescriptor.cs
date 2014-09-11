using System;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Module.Api.Operations.Pages.Pages.Search;
using BetterCms.Module.Search.Api.Operations.Pages.Pages;

namespace BetterCms.Module.Search.Api
{
    public class SearchApiModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public SearchApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
        }

        internal const string ModuleId = "a383b814-6dbf-497f-9f57-24fa57b74edd";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override Guid Id
        {
            get
            {
                return new Guid(ModuleId);
            }
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of API module.
        /// </value>
        public override string Name
        {
            get
            {
                return "search-api";
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
                return "A Search API module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 50;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SearchPagesService>().As<ISearchPagesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
        }
    }
}
