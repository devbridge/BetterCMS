using System.Collections.Generic;

using NuGet;

namespace BetterCms.Core.Modules.Install
{
    /// <summary>
    /// Defines the contract for module manager
    /// </summary>
    public interface IModuleManager
    {
        /// <summary>
        /// Get list of locally installed Modules.
        /// </summary>
        /// <returns>List of installed Modules</returns>
        IList<IPackage> GetInstalledModules();

        /// <summary>
        /// Get list of Modules available in remote Repository.
        /// </summary>
        /// <returns>List of modules.</returns>
        IList<IPackage> GetAvailableModules();

        /// <summary>
        /// Installs module.
        /// </summary>
        /// <param name="moduleId">Id of module</param>
        void InstallModule(string moduleId);

        /// <summary>
        /// Removes module.
        /// </summary>
        /// <param name="moduleId">Id of module</param>
        void RemoveModule(string moduleId);

        /// <summary>
        /// Get list of available updates for installed modules.
        /// </summary>
        /// <returns>List of modules</returns>
        IList<IPackage> CheckForUpdates();
    }
}
