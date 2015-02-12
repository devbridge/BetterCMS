using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Common.Logging;

using Devbridge.Platform.Core.Environment.FileSystem;
using Devbridge.Platform.Core.Exceptions;
using Devbridge.Platform.Core.Modules.Registration;

namespace Devbridge.Platform.Core.Environment.Assemblies
{
    /// <summary>
    /// Default assembly manager implementation.
    /// </summary>
    public class DefaultAssemblyManager : IAssemblyManager
    {               
        /// <summary>
        /// Logging contract.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Working directory managing.
        /// </summary>
        private readonly IWorkingDirectory workingDirectory;

        /// <summary>
        /// Modules registry.
        /// </summary>
        private readonly IModulesRegistration modulesRegistration;

        /// <summary>
        /// Assemblies loader.
        /// </summary>
        private readonly IAssemblyLoader assemblyLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAssemblyManager" /> class.
        /// </summary>
        /// <param name="workingDirectory">The working directory.</param>
        /// <param name="modulesRegistration">The module loader.</param>
        /// <param name="assemblyLoader">The assembly loader.</param>
        public DefaultAssemblyManager(
            IWorkingDirectory workingDirectory,
            IModulesRegistration modulesRegistration,
            IAssemblyLoader assemblyLoader)
        {
            this.workingDirectory = workingDirectory;
            this.modulesRegistration = modulesRegistration;
            this.assemblyLoader = assemblyLoader;
        }

        /// <summary>
        /// Tries to attach available module assemblies from working modules directory.
        /// </summary>
        public void AddUploadedModules()
        {
            if (Log.IsTraceEnabled)
            {
                Log.Trace("Add uploaded modules.");
            }

            var availableModuleFiles = workingDirectory.GetAvailableModules();
            var runtimeModuleFiles = new List<FileInfo>();

            foreach (var moduleInfo in availableModuleFiles)
            {
                try
                {
                    var runtimeModuleInfo = workingDirectory.RecopyModulesToRuntimeFolder(moduleInfo);
                    runtimeModuleFiles.Add(runtimeModuleInfo);
                }
                catch (Exception ex)
                {
                    throw new PlatformException("Failed to recopy module " + moduleInfo.FullName + ".", ex);
                }
            }

            foreach (var runtimeModuleFile in runtimeModuleFiles)
            {
                try
                {                    
                    if (runtimeModuleFile.FullName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        var runtimeModuleAssembly = assemblyLoader.Load(AssemblyName.GetAssemblyName(runtimeModuleFile.FullName));

                        AddUploadedModule(runtimeModuleAssembly);
                    }
                }
                catch (Exception ex)
                {
                    throw new PlatformException("Failed to add reference to runtime module " + runtimeModuleFile.FullName + ".", ex);
                }
            }
        }

        /// <summary>
        /// Adds the uploaded module.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public virtual void AddUploadedModule(Assembly assembly)
        {
            modulesRegistration.AddModuleDescriptorTypeFromAssembly(assembly);
        }

        /// <summary>
        /// Adds referenced modules.
        /// </summary>
        public void AddReferencedModules()
        {
            if (Log.IsTraceEnabled)
            {
                Log.Trace("Add referenced modules.");
            }

            var modules = AppDomain.CurrentDomain.GetAssemblies().Where(f => f.FullName.StartsWith("BetterCms.Module.", StringComparison.OrdinalIgnoreCase)).ToList();
            var loadedPaths = modules.Select(f => f.Location).ToArray();
            var referencedPaths = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"), "*.dll");
            var notLoadedReferencedPaths = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.OrdinalIgnoreCase)).ToList();

            foreach (var notLoadedReferencedPath in notLoadedReferencedPaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(notLoadedReferencedPath);
                if (fileName != null && fileName.StartsWith("BetterCms", StringComparison.OrdinalIgnoreCase))
                {
                    AssemblyName assemblyName = AssemblyName.GetAssemblyName(notLoadedReferencedPath);
                    if (assemblyName.FullName.StartsWith("BetterCms.Module.", StringComparison.OrdinalIgnoreCase))
                    {
                        var module = assemblyLoader.Load(assemblyName);
                        modules.Add(module);
                    }
                }
            }

            foreach (var module in modules)
            {
                AddReferencedModule(module);
            }
        }

        /// <summary>
        /// Adds the referenced module.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        public virtual void AddReferencedModule(Assembly assembly)
        {
            modulesRegistration.AddModuleDescriptorTypeFromAssembly(assembly);
        }
    }
}
