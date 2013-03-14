using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Autofac;

using BetterCms.Api;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Attributes;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;
using BetterCms.Core.Security;

namespace BetterCms.Core.Modules
{
    /// <summary>
    /// Abstract module descriptor. 
    /// </summary>
    public abstract class ModuleDescriptor
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public virtual int Order
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the name of the module area.
        /// </summary>
        /// <value>
        /// The name of the module area.
        /// </value>
        public virtual string AreaName
        {
            get
            {
                return "bcms-" + Name;
            }
        }

        /// <summary>
        /// Gets the base script path.
        /// </summary>
        /// <value>
        /// The base script path.
        /// </value>
        public virtual string BaseScriptPath
        {
            get
            {
                return string.Format("/file/{0}/scripts/", AreaName).ToLowerInvariant();
            }
        }
        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        /// <value>
        /// The name of the assembly.
        /// </value>        
        public AssemblyName AssemblyName
        {
            get
            {
                return GetType().Assembly.GetName();
            }
        }

        /// <summary>
        /// Registers a routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public virtual void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        public virtual void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {            
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The CMS configuration.</param>
        /// <returns>Enumerator of known JS modules list.</returns>
        public virtual IEnumerable<JavaScriptModuleDescriptor> RegisterJavaScriptModules(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarSideProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Enumerator of known module style sheet files.</returns>
        public virtual IEnumerable<string> RegisterStyleSheetFiles(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        /// <summary>
        /// Registers the permissions.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Enumerator of known module permissions.</returns>
        public virtual IEnumerable<IUserRole> RegisterUserRoles(ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            return null;
        }

        /// <summary>
        /// Registers module controller types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        internal void RegisterModuleControllers(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, ICmsConfiguration configuration, IControllerExtensions controllerExtensions)
        {
            var controllerTypes = controllerExtensions.GetControllerTypes(GetType().Assembly);

            if (controllerTypes != null)
            {
                var allModuleActions = new Dictionary<Type, IEnumerable<MethodInfo>>();
                foreach (Type controllerType in controllerTypes)
                {
                    string key = (AreaName + "-" + controllerType.Name).ToUpperInvariant();                    
                    containerBuilder
                        .RegisterType(controllerType)
                        .AsSelf()
                        .Keyed<IController>(key)                        
                        .WithMetadata("ControllerType", controllerType)
                        .InstancePerDependency()
                        .PropertiesAutowired(PropertyWiringFlags.PreserveSetValues);

                    var controllerActions = controllerExtensions.GetControllerActions(controllerType);

                    if (controllerActions != null)
                    {
                        allModuleActions.Add(controllerType, controllerActions);
                    }
                }

                foreach (var item in allModuleActions)
                {                 
                    var controllerName = controllerExtensions.GetControllerName(item.Key);
                    var controllerActions = item.Value;

                    foreach (var actionMethod in controllerActions)
                    {
                        var ignoreAutoRouteAttribute = actionMethod.GetCustomAttributes(typeof(IgnoreAutoRouteAttribute), false);
                        var nonActionAttribute = actionMethod.GetCustomAttributes(typeof(NonActionAttribute), false);
                        if (ignoreAutoRouteAttribute.Length > 0 || nonActionAttribute.Length > 0)
                        {
                            continue;
                        }
                        
                        registrationContext.MapRoute(
                            string.Format("bcms_{0}_{1}_{2}", AreaName, controllerName, actionMethod.Name),
                            string.Format("{0}/{1}/{2}", AreaName, controllerName, actionMethod.Name),
                            new
                            {
                                area = AreaName,
                                controller = controllerName,
                                action = actionMethod.Name
                            },
                            new[] { item.Key.Namespace });                        
                    }
                }
            }
        }

        /// <summary>
        /// Registers the module command types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The CMS configuration.</param>
        internal void RegisterModuleCommands(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            Assembly assembly = GetType().Assembly;

            Type[] commandTypes = new[]
                {
                    typeof(ICommand),
                    typeof(ICommand<>),
                    typeof(ICommand<,>)
                };

            containerBuilder
                .RegisterAssemblyTypes(assembly)
                .Where(scan => commandTypes.Any(commandType => IsAssignableToGenericType(scan, commandType)))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        internal void RegisterModuleApiContexts(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
        {
            Assembly assembly = GetType().Assembly;
            
            containerBuilder
                .RegisterAssemblyTypes(assembly)
                .AssignableTo(typeof(ApiContext))                
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        protected void RegisterContentRendererType<TContentRenderer, TContent>(ContainerBuilder containerBuilder) 
            where TContentRenderer : ContentAccessor<TContent>
            where TContent : class, IContent
        {
            Type contentRendererType = typeof(TContentRenderer);
            Type contentType = typeof(TContent);

            string key = ("ContentRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(contentRendererType)
                .AsSelf()
                .Keyed<IContentAccessor>(key)
                .WithMetadata("ContentRendererType", contentRendererType)               
                .InstancePerDependency();
        }

        protected void RegisterJavaScriptRendererType<TJavaScriptRenderer, TContent>(ContainerBuilder containerBuilder)
            where TJavaScriptRenderer : IJavaScriptAccessor
            where TContent : class
        {
            Type jsRendererType = typeof(TJavaScriptRenderer);
            Type contentType = typeof(TContent);

            string key = ("JavaScriptRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(jsRendererType)
                .AsSelf()
                .Keyed<IJavaScriptAccessor>(key)
                .WithMetadata("JavaScriptRendererType", jsRendererType)
                .InstancePerDependency();
        }
        
        protected void RegisterStylesheetRendererType<TStyleSheetRenderer, TContent>(ContainerBuilder containerBuilder)
            where TStyleSheetRenderer : IStylesheetAccessor
            where TContent : class
        {
            Type styleRendererType = typeof(TStyleSheetRenderer);
            Type contentType = typeof(TContent);

            string key = ("StyleSheetRenderer-" + contentType.Name).ToUpperInvariant();
            containerBuilder
                .RegisterType(styleRendererType)
                .AsSelf()
                .Keyed<IStylesheetAccessor>(key)
                .WithMetadata("StylesheetRendererType", styleRendererType)
                .InstancePerDependency();
        }

        /// <summary>
        /// Determines whether given type is assignable to generic type.
        /// </summary>
        /// <param name="givenType">A given type.</param>
        /// <param name="genericType">A generic type.</param>
        /// <returns>
        ///   <c>true</c> if given type is assignable to generic type; otherwise, <c>false</c>.
        /// </returns>
        private bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            Type baseType = givenType.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == genericType) || IsAssignableToGenericType(baseType, genericType);
        }
    }
}
