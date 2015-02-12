using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Autofac;

using Devbridge.Platform.Core.Extensions;
using Devbridge.Platform.Core.Modules;
using Devbridge.Platform.Core.Modules.Registration;
using Devbridge.Platform.Core.Web.Modules.Registration;
using Devbridge.Platform.Core.Web.Mvc.Commands;
using Devbridge.Platform.Core.Web.Mvc.Extensions;

namespace Devbridge.Platform.Core.Web.Modules
{
    /// <summary>
    /// Abstract web module descriptor. 
    /// </summary>
    public abstract class WebModuleDescriptor : ModuleDescriptor
    {
        private string areaName;

        private string baseModulePath;

        private string baseJsPath;

        private string baseCssPath;

        private string minJsPath;

        private string minCssPath;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public abstract override string Name { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public abstract override string Description { get; }

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
                    areaName = ("module-" + Name).ToLowerInvariant();
                }

                return areaName;
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
                    baseModulePath = VirtualPath.Combine("/", "file", AreaName);
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

        /// <summary>
        /// Gets the CSS base path.
        /// </summary>
        /// <value>
        /// The CSS base path.
        /// </value>
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
                    minJsPath = VirtualPath.Combine(JsBasePath, string.Format("module.{0}.min.js", Name.ToLowerInvariant()));
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
                    minCssPath = VirtualPath.Combine(CssBasePath, string.Format("module.{0}.min.css", Name.ToLowerInvariant()));
                }

                return minCssPath;
            }
        }

        /// <summary>
        /// Registers a routes.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public virtual void RegisterCustomRoutes(WebModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
        }

        /// <summary>
        /// Registers module controller types.
        /// </summary>
        /// <param name="registrationContext">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        /// <param name="controllerExtensions">The controller extensions.</param>
        public virtual void RegisterModuleControllers(WebModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder, IControllerExtensions controllerExtensions)
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
                        string.Format("module_{0}_internal_routes", AreaName),
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
        public virtual void RegisterModuleCommands(WebModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder)
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
                .Where(scan => commandTypes.Any(commandType => scan.IsAssignableToGenericType(commandType)))
                .AsImplementedInterfaces()
                .AsSelf()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Creates the registration context.
        /// </summary>
        /// <returns>Module registration context</returns>
        public override ModuleRegistrationContext CreateRegistrationContext()
        {
            return new WebModuleRegistrationContext(this);
        }
    }
}
