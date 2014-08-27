using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Autofac;

using BetterCms.Core.Modules;

namespace BetterCms.Module.GoogleAnalytics
{
    public class GoogleAnalyticsModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "google_analytics";

        private ICmsConfiguration _cmsConfiguration;

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
                return "The Google Analytics integration module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAnalyticsModuleDescriptor" class./>
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public GoogleAnalyticsModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            _cmsConfiguration = configuration;
        }

        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            context.MapRoute("bcms-google-sitemap", GoogleAnalyticsModuleHelper.GetSitemapUrl(_cmsConfiguration), new { area = AreaName, controller = "GoogleSitemap", action = "Index" });
        }

    }
}