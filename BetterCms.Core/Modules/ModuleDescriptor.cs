using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Autofac;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Mvc.Extensions;

namespace BetterCms.Core.Modules
{
    /// <summary>
    /// Abstract module descriptor. 
    /// </summary>
    public abstract class ModuleDescriptor
    {
        private string areaName;

        private string baseModulePath;

        private string baseJsPath;

        private string baseCssPath;

        private string minJsPath;

        private string minCssPath;

        private AssemblyName assemblyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        protected ModuleDescriptor(ICmsConfiguration configuration)
        {
            Configuration = configuration;
        }

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
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public abstract Guid Id { get; }

        /// <summary>
        /// Flag describe is module root or additional
        /// </summary>
        public virtual bool IsRootModule
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public virtual ICmsConfiguration Configuration { get; private set; }

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
                if (areaName == null)
                {
                    areaName = ("bcms-" + Name).ToLowerInvariant();
                }

                return areaName;
            }
        }
        
        /// <summary>
        /// Gets the name of the module database schema name.
        /// </summary>
        /// <value>
        /// The name of the module database schema.
        /// </value>
        public virtual string SchemaName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the base module path. Default value is /file/module-name/.
        /// </summary>
        /// <value>
        /// The base module path.
        /// </value>
        public virtual string BaseModulePath
        {
            get
            {
                if (baseModulePath == null)
                {                    
                    if (!string.IsNullOrWhiteSpace(Configuration.ResourcesBasePath) && !VirtualPath.IsLocalPath(Configuration.ResourcesBasePath))
                    {                       
                        baseModulePath = VirtualPath.Combine(Configuration.ResourcesBasePath, AreaName);

                        const string versionTag = "{bcms.version}";
                        if (baseModulePath.Contains(versionTag))
                        {
                            baseModulePath = baseModulePath.Replace(versionTag, Configuration.Version);
                        }
                    }
                    else
                    {
                        if (Configuration.UseMinifiedResources)
                        {
                            throw new CmsException(
                                "The switch useMinifiedResources=\"true\" in the cms.config can't be used with local CMS resources (resourcesBasePath=\"(local)\").");
                        }
                        baseModulePath = VirtualPath.Combine("/", "file", AreaName);
                    }
                }

                return baseModulePath;
            }
        }

        /// <summary>
        /// Gets the JavaScript base path.
        /// </summary>
        /// <value>
        /// The JavaScript base path.
        /// </value>
        public virtual string JsBasePath
        {
            get
            {
                if (baseJsPath == null)
                {
                    baseJsPath = VirtualPath.Combine(BaseModulePath, "scripts");
                }

                return baseJsPath;
            }
        }

        public virtual string CssBasePath
        {
            get
            {
                if (baseCssPath == null)
                {
                    baseCssPath = VirtualPath.Combine(BaseModulePath, "content", "styles");
                }

                return baseCssPath;
            }
        }

        /// <summary>
        /// Gets the path of the module packed and minified JS file.
        /// </summary>
        /// <value>
        /// The path of the module packed and minified JS file.
        /// </value>
        public virtual string MinifiedJsPath
        {
            get
            {
                if (minJsPath == null)
                {
                    minJsPath = VirtualPath.Combine(JsBasePath, string.Format("bcms.{0}.min.js", Name.ToLowerInvariant()));
                }

                return minJsPath;
            }
        }

        /// <summary>
        /// Gets the path of the module packed and minified CSS file.
        /// </summary>
        /// <value>
        /// The path of the module packed and minified CSS file.
        /// </value>
        public virtual string MinifiedCssPath
        {
            get
            {
                if (minCssPath == null)
                {
                    minCssPath = VirtualPath.Combine(CssBasePath, string.Format("bcms.{0}.min.css", Name.ToLowerInvariant()));
                }

                return minCssPath;
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
                if (assemblyName == null)
                {
                    assemblyName = GetType().Assembly.GetName();
                }

                return assemblyName;
            }
        }

        /// <summary>
        /// Registers a routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public virtual void RegisterCustomRoutes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public virtual void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {            
        }

        /// <summary>
        /// Registers java script modules.
        /// </summary>        
        /// <returns>Enumerator of known JS modules list.</returns>
        public virtual IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return null;
        }

        /// <summary>
        /// Registers the style sheet files.
        /// </summary>                
        /// <returns>Enumerator of known module style sheet files.</returns>
        public virtual IEnumerable<CssIncludeDescriptor> RegisterCssIncludes()
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarHeaderProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarSideProjections(ContainerBuilder containerBuilde)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSidebarMainProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        public virtual IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return null;
        }

        /// <summary>
        /// Registers module controller types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        internal void RegisterModuleControllers(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, IControllerExtensions controllerExtensions)
        {
            var controllerTypes = controllerExtensions.GetControllerTypes(GetType().Assembly);

            if (controllerTypes != null)
            {
                var namespaces = new List<string>();

                foreach (Type controllerType in controllerTypes)
                {
                    string key = (AreaName + "-" + controllerType.Name).ToUpperInvariant();
                    if (!namespaces.Contains(controllerType.Namespace))
                    {
                        namespaces.Add(controllerType.Namespace);
                    }

                    containerBuilder
                        .RegisterType(controllerType)
                        .AsSelf()
                        .Keyed<IController>(key)
                        .WithMetadata("ControllerType", controllerType)
                        .InstancePerDependency()
                        .PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);                                                           
                }

                registrationContext.MapRoute(
                        string.Format("bcms_{0}_internal_routes", AreaName),
                        string.Format("{0}/{{controller}}/{{action}}", AreaName),
                        new
                            {
                                area = AreaName
                            },
                        namespaces.ToArray());
            }
        }
        
        /// <summary>
        /// Registers the module command types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        internal void RegisterModuleCommands(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder)
        {
            Assembly assembly = GetType().Assembly;

            Type[] commandTypes = new[]
                {
                    typeof(ICommand),
                    typeof(ICommandIn<>),
                    typeof(ICommandOut<>),
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
