using System;
using System.Collections.Generic;
using System.Linq;
using BetterCms.Core.Exceptions.Modules;
using NuGet;

namespace BetterCms.Core.Modules.Install
{
    public class DefaultModuleManager : IModuleManager
    {
        private readonly IPackageRepository packageRepository;

        private readonly IPackageManager packageManager;
        
        public DefaultModuleManager(IPackageRepository packageRepository, ICmsConfiguration cmsConfiguration)
        {
            this.packageRepository = PackageRepositoryFactory.Default.CreateRepository(cmsConfiguration.ModuleGallery.FeedUrl);
            this.packageManager = new PackageManager(
                this.packageRepository,
                cmsConfiguration.WorkingDirectoryRootPath) { Logger = null };
        }

        /// <summary>
        /// Get list of locally installed Modules.
        /// </summary>
        /// <returns>List of installed Modules</returns>
        public IList<IPackage> GetInstalledModules()
        {
            CheckRepository();

            return packageManager.LocalRepository
                .GetPackages()
                .OrderBy(x => x.Id)
                .ToList();
        }

        /// <summary>
        /// Get list of Modules available in remote Repository.
        /// </summary>
        /// <returns>List of modules.</returns>
        public IList<IPackage> GetAvailableModules()
        {
            CheckRepository();

            return packageManager.SourceRepository
                .GetPackages()
                .OrderBy(x => x.Id)
                .ToList();
        }

        /// <summary>
        /// Installs module.
        /// </summary>
        /// <param name="moduleId">Id of module</param>
        public void InstallModule(string moduleId)
        {
            if (string.IsNullOrWhiteSpace(moduleId))
            {
                throw new ArgumentNullException("moduleId");
            }

            CheckRepository();

            var installedModule = packageManager.LocalRepository
                .FindPackagesById(moduleId)
                .OrderByDescending(x => x.Version.Version)
                .FirstOrDefault();

            var module = packageManager.SourceRepository
                .FindPackagesById(moduleId)
                .OrderByDescending(x => x.Version.Version)
                .FirstOrDefault();
            
            if (module != null && installedModule == null)
            {
                packageManager.InstallPackage(module.Id, module.Version, false, true);
            }
            else if (module != null && installedModule != null)
            {
                int result = module.Version.Version.CompareTo(installedModule.Version.Version);

                if (result == 0)
                {
                    throw new ModuleAlreadyInstalledException(
                        string.Format("Module Id: '{0}' version: '{1}' already installed!", module.Id, module.Version));
                }
                else
                {
                    throw new ModuleAlreadyInstalledException(
                        string.Format(
                            "Module Id: '{0}' version: '{1}' already installed! Please remove other version and tray gain!", 
                            installedModule.Id, 
                            installedModule.Version));
                }
            }
            else
            {
                throw new ModuleNotFoundException(string.Format("Module Id: '{0}' not found!", moduleId));
            }
        }

        /// <summary>
        /// Removes module.
        /// </summary>
        /// <param name="moduleId">Id of module</param>
        public void RemoveModule(string moduleId)
        {
            if (string.IsNullOrWhiteSpace(moduleId))
            {
                throw new ArgumentNullException("moduleId");
            }

            CheckRepository();

            var module = packageManager.LocalRepository
                .FindPackagesById(moduleId)
                .OrderByDescending(x => x.Version.Version)
                .FirstOrDefault();

            if (module == null)
            {
                throw new ModuleNotFoundException(string.Format("Module Id: '{0}' not found!", moduleId));
            } 
            
            packageManager.LocalRepository.RemovePackage(module);
        }

        /// <summary>
        /// Get list of available updates for installed modules.
        /// </summary>
        /// <returns>List of modules</returns>
        public IList<IPackage> CheckForUpdates()
        {
            CheckRepository();

            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if repository and manager is initialized.
        /// </summary>
        private void CheckRepository()
        {
            if (packageRepository == null)
            {
                throw new ModuleRepositoryNullException("Module Repository not initialized!");
            }

            if (packageManager == null)
            {
                throw new ModuleManagerNullException("Module Manager not initialized!");
            }            
        }
    }
}
