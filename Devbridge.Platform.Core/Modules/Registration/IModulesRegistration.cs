using System.Collections.Generic;
using System.Reflection;

namespace Devbridge.Platform.Core.Modules.Registration
{
    /// <summary>
    /// Defines the contract for modules registration logic.
    /// </summary>
    public interface IModulesRegistration
    {
        /// <summary>
        /// Tries to scan and register module type from assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        void AddModuleDescriptorTypeFromAssembly(Assembly assembly);
        
        /// <summary>
        /// Gets registered modules.
        /// </summary>
        /// <returns>Enumerator of registered modules.</returns>
        IEnumerable<ModuleRegistrationContext> GetModules();
        
        /// <summary>
        /// Starts known modules.
        /// </summary>        
        void InitializeModules();
    }
}
