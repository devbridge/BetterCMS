using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Autofac;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;

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
        /// Registers module controller types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public virtual void RegisterModuleControllers(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, ICmsConfiguration configuration, IControllerExtensions controllerExtensions)
        {
            var controllerTypes = controllerExtensions.GetControllerTypes(GetType().Assembly);

            if (controllerTypes != null)
            {
                var allModuleActions = new Dictionary<Type, IEnumerable<string>>();
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
                    var actionNames = item.Value;

                    foreach (var actionName in actionNames)
                    {
                        registrationContext.MapRoute(
                            string.Format("bcms_{0}_{1}_{2}", AreaName, controllerName, actionName),
                            string.Format("{0}/{1}/{2}", AreaName, controllerName, actionName),
                            new
                            {
                                area = AreaName,
                                controller = controllerName,
                                action = actionName
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
        public virtual void RegisterModuleCommands(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, ICmsConfiguration configuration)
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
