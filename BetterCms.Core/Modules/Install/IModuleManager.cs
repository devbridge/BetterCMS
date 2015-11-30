// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModuleManager.cs" company="Devbridge Group LLC">
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
