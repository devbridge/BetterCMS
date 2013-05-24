using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Autofac;

using BetterCms.Core.Models;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
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
        /// The users area name.
        /// </summary>
        internal const string UsersAreaName = "bcms-users";

        /// <summary>
        /// The user java script module descriptor
        /// </summary>
        private readonly UserJsModuleIncludeDescriptor userJsModuleIncludeDescriptor;

         /// <summary>
        /// Initializes a new instance of the <see cref="UsersModuleDescriptor" /> class.
        /// </summary>
        public UsersModuleDescriptor(ICmsConfiguration configuration) : base(configuration)
        {
            userJsModuleIncludeDescriptor = new UserJsModuleIncludeDescriptor(this);
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
                return "Users module for Better CMS.";
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
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public override string AreaName
        {
            get
            {
                return UsersAreaName;
            }
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>        
        /// <returns>Enumerator of known module style sheet files.</returns>
        public override IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return new[]
                {
                    new CssIncludeDescriptor(this, "bcms.users.css") 
                };
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new JsIncludeDescriptor[]
                {
                    userJsModuleIncludeDescriptor,
                    new RoleJsModuleIncludeDescriptor(this) 
                };
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
                    new LinkActionProjection(userJsModuleIncludeDescriptor, page => "loadSiteSettingsUsers")
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
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<DefaultRoleService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultAuthentictionService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
