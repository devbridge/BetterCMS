using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Navigation.Content.Resources;
using BetterCms.Module.Navigation.Registration;
using BetterCms.Module.Navigation.Services;

namespace BetterCms.Module.Navigation
{
    /// <summary>
    /// A navigation module descriptor.
    /// </summary>
    public class NavigationModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "navigation";

        /// <summary>
        /// bcms.sitemap.js java script module descriptor.
        /// </summary>
        private readonly SitemapJavaScriptModuleDescriptor sitemapJavaScriptModuleDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationModuleDescriptor" /> class.
        /// </summary>
        public NavigationModuleDescriptor()
        {
            sitemapJavaScriptModuleDescriptor = new SitemapJavaScriptModuleDescriptor(this);
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
                return "Sitemap module for BetterCMS.";
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            containerBuilder.RegisterType<DefaultSitemapService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<string> RegisterStyleSheetFiles(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new[]
                {
                    "/file/bcms-navigation/Content/Css/bcms.navigation.css"
                };
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The CMS configuration.</param>
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JavaScriptModuleDescriptor> RegisterJavaScriptModules(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new JavaScriptModuleDescriptor[]
                {
                    sitemapJavaScriptModuleDescriptor
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns list of the <see cref="IPageActionProjection" /> items.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(sitemapJavaScriptModuleDescriptor, page => "loadSiteSettingsSitemap")
                        {
                            Order = 4500,
                            Title = () => NavigationGlobalization.SiteSettings_SitemapMenuItem,
                            CssClass = page => "bcms-sidebar-link"
                        }                                      
                };
        }
    }
}
