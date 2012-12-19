using System;

using BetterCms.Core.Environment.Assemblies;
using BetterCms.Core.Modules.Registration;

using Common.Logging;
using FluentNHibernate.Cfg;

namespace BetterCms.Core.DataAccess.DataContext
{
    public class DefaultMappingResolver : IMappingResolver
    {
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        private readonly IModulesRegistration modulesRegistry;
        private readonly IAssemblyLoader assemblyLoader;

        public DefaultMappingResolver(IModulesRegistration modulesRegistry, IAssemblyLoader assemblyLoader)
        {        
            this.modulesRegistry = modulesRegistry;
            this.assemblyLoader = assemblyLoader;
        }

        public void AddAvailableMappings(FluentConfiguration fluentConfiguration)
        {
            foreach (var module in modulesRegistry.GetModules())
            {
                try
                {
                    var assembly = assemblyLoader.Load(module.ModuleDescriptor.AssemblyName);
                    if (assembly != null)
                    {
                        fluentConfiguration = fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
                    }
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("Failed to load mappings from module {0} (assembly {1}).", ex, module.ModuleDescriptor.Name, module.ModuleDescriptor.AssemblyName);
                }
            }
        }
    }
}
