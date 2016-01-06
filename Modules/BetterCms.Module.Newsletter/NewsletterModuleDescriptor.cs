using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Registration;
using BetterCms.Module.Newsletter.Services;
using BetterCms.Module.Root;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Newsletter
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class NewsletterModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "newsletter";

        /// <summary>
        /// The newsletter area name.
        /// </summary>
        internal const string NewsletterAreaName = "bcms-newsletter";

        /// <summary>
        /// The newsletter module databse schema name
        /// </summary>
        internal const string NewsletterSchemaName = "bcms_newsletter";

        /// <summary>
        /// The newsletter java script module descriptor.
        /// </summary>
        private readonly NewsletterJsModuleIncludeDescriptor newsletterJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterModuleDescriptor" /> class.
        /// </summary>
        public NewsletterModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            newsletterJsModuleIncludeDescriptor = new NewsletterJsModuleIncludeDescriptor(this);
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
                return "A newsletter module for Better CMS.";
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
                return NewsletterAreaName;
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
                return NewsletterSchemaName;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultSubscriberService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>        
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[] { newsletterJsModuleIncludeDescriptor };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new SeparatorProjection(9999),
                    new LinkActionProjection(newsletterJsModuleIncludeDescriptor, page => "loadSiteSettingsNewsletterSubscribers")
                        {
                            Order = 9999,
                            Title = page => NewsletterGlobalization.SiteSettings_NewsletterSubscribersMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                        }
                };
        }
    }
}
