using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Autofac;

using Common.Logging;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Environment.Assemblies;

namespace Devbridge.Platform.Core.Modules.Registration
{
    /// <summary>
    /// Default modules registration implementation.
    /// </summary>
    public class DefaultModulesRegistration : IModulesRegistration
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        protected static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Assembly loader instance.
        /// </summary>
        protected readonly IAssemblyLoader assemblyLoader;

        /// <summary>
        /// Thread safe known module types.
        /// </summary>
        protected readonly Dictionary<string, Type> knownModuleDescriptorTypes;

        /// <summary>
        /// Thread safe modules dictionary.
        /// </summary>
        protected readonly Dictionary<string, ModuleRegistrationContext> knownModules;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModulesRegistration" /> class.
        /// </summary>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public DefaultModulesRegistration(IAssemblyLoader assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;

            knownModuleDescriptorTypes = new Dictionary<string, Type>();
            knownModules = new Dictionary<string, ModuleRegistrationContext>();
        }

        /// <summary>
        /// Tries to scan and adds module descriptor type from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        public void AddModuleDescriptorTypeFromAssembly(Assembly assembly)
        {
            if (Log.IsTraceEnabled)
            {
                Log.TraceFormat("Searching for module descriptor type in the assembly {0}.", assembly.FullName);
            }
      
            var moduleRegistrationType = assemblyLoader.GetLoadableTypes(assembly).Where(IsModuleDescriptorType).FirstOrDefault();
            if (moduleRegistrationType != null)
            {
                if (Log.IsTraceEnabled)
                {
                    Log.TraceFormat("Adds module descriptor {0} from the assembly {1}.", moduleRegistrationType.Name, assembly.FullName);
                }

                if (!knownModuleDescriptorTypes.ContainsKey(moduleRegistrationType.Name))
                {
                    knownModuleDescriptorTypes.Add(moduleRegistrationType.Name, moduleRegistrationType);
                }
                else
                {
                    Log.InfoFormat("Module descriptor {0} from the assembly {1} already included.", moduleRegistrationType.Name, assembly.FullName);
                }
            }
        }

        /// <summary>
        /// Gets registered modules.
        /// </summary>
        /// <returns>List of registered modules.</returns>
        public IEnumerable<ModuleRegistrationContext> GetModules()
        {
            return knownModules.Values;
        }
        
        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="moduleDescriptor">Module information.</param>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private void RegisterModule(ModuleDescriptor moduleDescriptor)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            var registrationContext = moduleDescriptor.CreateRegistrationContext();

            RegisterModuleDescriptor(registrationContext, containerBuilder);
            ContextScopeProvider.RegisterTypes(containerBuilder);

            knownModules.Add(registrationContext.GetRegistrationName(), registrationContext);
        }

        /// <summary>
        /// Registers the types.
        /// </summary>
        /// <param name="registrationContext">The registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        protected virtual void RegisterModuleDescriptor(ModuleRegistrationContext registrationContext, ContainerBuilder containerBuilder)
        {
            registrationContext.ModuleDescriptor.RegisterModuleTypes(registrationContext, containerBuilder);
        }

        /// <summary>
        /// Initializes all known modules.
        /// </summary>        
        public void InitializeModules()
        {
            if (knownModuleDescriptorTypes != null && knownModuleDescriptorTypes.Count > 0)
            {
                ContainerBuilder containerBuilder = new ContainerBuilder();
                foreach (var moduleDescriptorType in knownModuleDescriptorTypes.Values)
                {
                    containerBuilder.RegisterType(moduleDescriptorType).AsSelf().SingleInstance();
                }

                ContextScopeProvider.RegisterTypes(containerBuilder);

                using (var container = ContextScopeProvider.CreateChildContainer())
                {
                    var moduleDescriptors = new List<ModuleDescriptor>();
                    foreach (var moduleDescriptorType in knownModuleDescriptorTypes.Values)
                    {
                        if (container.IsRegistered(moduleDescriptorType))
                        {
                            var moduleDescriptor = container.Resolve(moduleDescriptorType) as ModuleDescriptor;
                            moduleDescriptors.Add(moduleDescriptor);
                        }
                        else
                        {
                            Log.WarnFormat("Failed to resolve module instance from type {0}.", moduleDescriptorType.FullName);
                        }
                    }

                    moduleDescriptors = moduleDescriptors.OrderBy(f => f.Order).ToList();
                    foreach (var moduleDescriptor in moduleDescriptors)
                    {
                        try
                        {
                            RegisterModule(moduleDescriptor);                            
                        }
                        catch (Exception ex)
                        {
                            Log.ErrorFormat("Failed to register module of type {0}.", ex, moduleDescriptor.GetType().FullName);
                        }
                    }
                }
            }
            else
            {
                Log.Info("No registered module descriptors found.");
            }
        }

        /// <summary>
        /// Determines whether type is module descriptor type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if type is module descriptor type; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsModuleDescriptorType(Type type)
        {
            return typeof(ModuleDescriptor).IsAssignableFrom(type) && type.IsPublic;
        }
    }
}
