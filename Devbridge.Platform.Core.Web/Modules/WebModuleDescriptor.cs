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
