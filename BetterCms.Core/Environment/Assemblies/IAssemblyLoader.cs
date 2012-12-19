using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace BetterCms.Core.Environment.Assemblies
{
    /// <summary>
    /// Defines the contract to load assembly.
    /// </summary>
    public interface IAssemblyLoader
    {
        /// <summary>
        /// Loads assembly from file.
        /// </summary>
        /// <param name="assemblyFileName">Assembly file path.</param>
        /// <returns>Assembly object.</returns>
        Assembly LoadFromFile(string assemblyFileName);

        /// <summary>
        /// Loads the specified assembly by assembly name.
        /// </summary>
        /// <param name="assemblyName">A name of the assembly.</param>
        /// <returns>Assembly object.</returns>
        Assembly Load(AssemblyName assemblyName);

        /// <summary>
        /// Gets loadable types from assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Collection of types in the assembly.</returns>
        IEnumerable<Type> GetLoadableTypes(Assembly assembly);

        /// <summary>
        /// Gets loadable types from assembly with base type filter.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="ofBaseType">Type of the of base type.</param>
        /// <returns>Collection of types in the assembly.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<Type> GetLoadableTypes(Assembly assembly, Type ofBaseType);
    }
}
