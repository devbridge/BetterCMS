using System;
using System.Collections.Generic;

using Autofac;

using BetterCms.Configuration.Dynamic;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Core.Modules;
using BetterCms.Events;
using BetterCms.Module.GoogleAnalytics.Accessors;
using BetterCms.Module.GoogleAnalytics.Content.Resources;

namespace BetterCms.Module.GoogleAnalytics
{
    public class GoogleAnalyticsModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "google_analytics";

        internal const string ModuleId = "de5280e4-99e6-4c95-ac06-1c31312b82ec";

        private readonly ICmsConfiguration cmsConfiguration;

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
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "The Google Analytics integration module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAnalyticsModuleDescriptor"/> class
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public GoogleAnalyticsModuleDescriptor(ICmsConfiguration configuration)//, ICmsConfigurationService cmsConfigurationService)
            : base(configuration)
        {
            cmsConfiguration = configuration;
//            this.cmsConfigurationService = cmsConfigurationService;
            RootEvents.Instance.PageRendering += Events_PageRendering;
        }

        /// <summary>
        /// Register a routes for the google analytics module.
        /// </summary>
        /// <param name="context">The module context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            context.MapRoute(
                "bcms-google-sitemap",
                GoogleAnalyticsModuleHelper.GetSitemapUrl(cmsConfiguration),
                new { area = AreaName, controller = "GoogleSitemap", action = "Index" });
        }

        protected override IEnumerable<ConfigurationKeyValueDescriptor> RegisteredConfigurationSettings()
        {
            return new List<ConfigurationKeyValueDescriptor>
            {
                new ConfigurationKeyValueDescriptor
                {
                    Name = GoogleAnalyticsModuleConstants.SitemapTitleKey,
                    Value = "Default Site Map",
                    Priority = 1,
                    Title = () => GoogleAnalyticsGlobalization.SitemapTitle_DisplayName,
                    Type = OptionType.Text,
                    //Map = (configuration, value) => configuration.Storage.MaximumFileNameLength = value
                },
                new ConfigurationKeyValueDescriptor
                {
                    Name = GoogleAnalyticsModuleConstants.PriorityKey,
                    Value = "0.6",
                    Priority = 2,
                    Title = () => "Priority",
                    Type = OptionType.Text
                },
                new ConfigurationKeyValueDescriptor
                {
                    Name = GoogleAnalyticsModuleConstants.SitemapUrlKey,
                    Value = "sitemap1.xml",
                    Priority = 3,
                    Title = () => GoogleAnalyticsGlobalization.SitemapUrl_DisplayName,
                    Type = OptionType.Text
                }
            };
        }

        /// <summary>
        /// Add google analytics script accessor to Page.
        /// </summary>
        /// <param name="args">The args.</param>
        private void Events_PageRendering(PageRenderingEventArgs args)
        {
            args.RenderPageData.JavaScripts.Add(new GoogleAnalyticsScriptAccessor(cmsConfiguration));
        }

    }
}