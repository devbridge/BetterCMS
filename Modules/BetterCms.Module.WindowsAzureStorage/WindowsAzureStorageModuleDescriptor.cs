// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WindowsAzureStorageModuleDescriptor.cs" company="Devbridge Group LLC">
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
using Autofac;

using BetterCms.Configuration;
using BetterCms.Core.Modules;
using BetterCms.Core.Services.Storage;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.WindowsAzureStorage
{
    /// <summary>
    /// A storage module based on the Windows Azure Storage cloud service.
    /// </summary>
    public class WindowsAzureStorageModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public override string Name
        {
            get
            {
                return "WindowsAzureStorage";
            }
        }
         
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "A storage module based on Windows Azure Storage cloud service.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsAzureStorageModuleDescriptor" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public WindowsAzureStorageModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            if (Configuration.Storage.ServiceType == StorageServiceType.Auto)
            {
                // TODO: check if Storage module is registered if throw cmsexception.                
                containerBuilder.RegisterType<WindowsAzureStorageService>().As<IStorageService>().SingleInstance();
            }
        }
    }
}
