using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Models;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.JsModule;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Blog.Registration;
using BetterCms.Module.Root;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Registration;
using BetterCms.Module.Users.Services;

namespace BetterCms.Module.Users
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class UsersModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "users";
        /// <summary>
        /// The user java script module descriptor
        /// </summary>
        private readonly UserJavaScriptModuleDescriptor userJavaScriptModuleDescriptor;

         /// <summary>
        /// Initializes a new instance of the <see cref="UsersModuleDescriptor" /> class.
        /// </summary>
        public UsersModuleDescriptor()
        {
            userJavaScriptModuleDescriptor = new UserJavaScriptModuleDescriptor(this);
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
                return "Users module for BetterCMS.";
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
                return int.MaxValue - 200;
            }
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
                    "/file/bcms-users/Content/Css/bcms.users.css"
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
                    userJavaScriptModuleDescriptor,
                    new RoleJavaScriptModuleDescriptor(this) 
                };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return new IPageActionProjection[]
                {
                    new LinkActionProjection(userJavaScriptModuleDescriptor, page => "loadSiteSettingsUsers")
                        {
                            Order = 4100,
                            Title = () => UsersGlobalization.SiteSettings_UserMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.Administration
                        }                                      
                };
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            containerBuilder.RegisterType<DefaultRoleService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAuthentictionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
