// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultModuleManager.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
                throw new ArgumentNullException(nameof(moduleId));
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
                throw new ArgumentNullException(nameof(moduleId));
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
