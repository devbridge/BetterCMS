using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Common.Logging;

namespace BetterCms.Core.Environment.Assemblies
{
    /// <summary>
    /// Provides functionality to load assemblies.
    /// </summary>
    public class DefaultAssemblyLoader : IAssemblyLoader
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Loaded assemblies container.
        /// </summary>
        private readonly ConcurrentDictionary<string, Assembly> loadedAssemblies;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAssemblyLoader" /> class.
        /// </summary>
        public DefaultAssemblyLoader()
        {
            loadedAssemblies = new ConcurrentDictionary<string, Assembly>();
        }

        /// <summary>
        /// Loads assembly from file.
        /// </summary>
        /// <param name="assemblyFileName">Assembly file path.</param>
        /// <returns>Assembly object.</returns>
        public Assembly LoadFromFile(string assemblyFileName)
        {
            var assemblyName = AssemblyName.GetAssemblyName(assemblyFileName);
            return loadedAssemblies.GetOrAdd(assemblyName.FullName, a => Assembly.Load(assemblyName.FullName));           
        }

        /// <summary>
        /// Loads the specified assembly by assembly full name.
        /// </summary>
        /// <param name="assemblyName">Full name of the assembly.</param>
        /// <returns>Assembly object.</returns>
        public Assembly Load(AssemblyName assemblyName)
        {
            return loadedAssemblies.GetOrAdd(assemblyName.FullName, a => Assembly.Load(assemblyName.FullName));
        }

        /// <summary>
        /// Gets loadable types from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Collection of types in assembly.</returns>
        public IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            try
            {
                return assembly.GetExportedTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                Logger.WarnFormat("Failed to scan loadable types from assembly {0}.", ex, assembly.FullName);

                return ex.Types.Where(t => t != null);
            }
        }

        /// <summary>
        /// Gets loadable types from assembly with base type filter.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="ofBaseType">Type of the of base type.</param>
        /// <returns>Collection of types in the assembly.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public IEnumerable<Type> GetLoadableTypes(Assembly assembly, Type ofBaseType)
        {
            var types = GetLoadableTypes(assembly);
            return types != null
                       ? types.Where(ofBaseType.IsAssignableFrom)
                       : null;
        }
    }
}