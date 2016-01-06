using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web;

using Autofac;

using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Services;

using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Registration;
using BetterCms.Module.Users.Services;

using BetterModules.Core.Dependencies;
using BetterModules.Core.Modules.Registration;
using BetterModules.Core.Web.Modules.Registration;
using BetterModules.Events;

namespace BetterCms.Module.Users
{
    using Common.Logging;

    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class UsersModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "users";

        /// <summary>
        /// The users module area name.
        /// </summary>
        internal const string UsersAreaName = "bcms-users";

        /// <summary>
        /// The users module database schema name
        /// </summary>
        internal const string UsersSchemaName = "bcms_users";

        /// <summary>
        /// The user java script module descriptor
        /// </summary>
        private readonly UserJsModuleIncludeDescriptor userJsModuleIncludeDescriptor;

        /// <summary>
        /// Determines, if first user is registered
        /// </summary>
        private bool isFirstUserRegistered;

        /// <summary>
        /// Gets a value indicating whether first user is already registered.
        /// </summary>
        /// <value>
        /// <c>true</c> if first user is already registered; otherwise, <c>false</c>.
        /// </value>
        public bool IsFirstUserRegistered
        {
            get
            {
                return isFirstUserRegistered;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersModuleDescriptor" /> class.
        /// </summary>
        public UsersModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            userJsModuleIncludeDescriptor = new UserJsModuleIncludeDescriptor(this);

            WebCoreEvents.Instance.HostStart += OnHostStart;
            WebCoreEvents.Instance.HostAuthenticateRequest += HostAuthenticateRequest;            
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
                return int.MaxValue - 250;
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
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public override string SchemaName
        {
            get
            {
                return UsersSchemaName;
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
                            Title = page => UsersGlobalization.SiteSettings_UserMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.ManageUsers
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
            containerBuilder.RegisterType<DefaultAuthenticationService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultRoleService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultUserService>().AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultUserProfileUrlResolver>().As<IUserProfileUrlResolver>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DefaultRegistrationService>().As<IRegistrationService>().InstancePerLifetimeScope();
        }

        public override void RegisterCustomRoutes(WebModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            if (Configuration.Users != null)
            {
                if (Configuration.Users.CreateDefaultUserOnStart)
                {
                    context.MapRoute(
                        "bcms-users-register-first-user",
                        AreaName + "/register",
                        new
                            {
                                area = AreaName,
                                controller = "Registration",
                                action = "CreateFirstUser"
                            });
                }

                if (Configuration.Users.EnableCmsFormsAuthentication)
                {
                    context.MapRoute(
                        "bcms-users-login",
                        "login",
                        new
                        {
                            area = AreaName,
                            controller = "Authentication",
                            action = "Login"
                        });

                    context.MapRoute(
                        "bcms-users-logout",
                        "logout",
                        new
                        {
                            area = AreaName,
                            controller = "Authentication",
                            action = "Logout"
                        });                 
                }
            }
        }

        private void OnHostStart(SingleItemEventArgs<HttpApplication> args)
        {
            Logger.Info("OnHostStart: check if is first user registered...");

            if (Configuration.Users != null && Configuration.Users.CreateDefaultUserOnStart)
            {
                CheckIfIsFirstUserRegistered();
            }

            Logger.Info("OnHostStart: checking if is first user registered completed.");
        }

        private void HostAuthenticateRequest(SingleItemEventArgs<HttpApplication> args)
        {
            if (Configuration.Users != null)
            {
                if (Configuration.Users.CreateDefaultUserOnStart)
                {
                    CheckIfIsFirstUserRegistered();

                    if (!isFirstUserRegistered)
                    {
                        using (var container = ContextScopeProvider.CreateChildContainer())
                        {
                            var registrationService = container.Resolve<IRegistrationService>();
                            registrationService.NavigateToRegisterFirstUserPage();
                        }
                    }
                }
            }
        }        
        
        private void CheckIfIsFirstUserRegistered()
        {
            if (!isFirstUserRegistered)
            {
                lock (this)
                {
                    if (!isFirstUserRegistered )
                    {
                        using (var container = ContextScopeProvider.CreateChildContainer())
                        {
                            var registrationService = container.Resolve<IRegistrationService>();
                            isFirstUserRegistered = registrationService.IsFirstUserRegistered();
                        }
                    }
                }
            }
        }

        internal void SetAsFirstUserRegistered()
        {
            lock (this)
            {
                isFirstUserRegistered = true;
            }
        }
    }
}
