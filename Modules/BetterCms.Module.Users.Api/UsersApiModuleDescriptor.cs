// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersApiModuleDescriptor.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Modules;

using BetterCms.Module.Api.Operations.Users.Roles;
using BetterCms.Module.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Api.Operations.Users.Users;
using BetterCms.Module.Api.Operations.Users.Users.User;
using BetterCms.Module.Api.Operations.Users.Users.User.ValidateUser;

using BetterCms.Module.Users.Api.Operations.Users.Roles;
using BetterCms.Module.Users.Api.Operations.Users.Roles.Role;
using BetterCms.Module.Users.Api.Operations.Users.Users;
using BetterCms.Module.Users.Api.Operations.Users.Users.User;
using BetterCms.Module.Users.Api.Operations.Users.Users.User.Validate;

using BetterModules.Core.Modules.Registration;

namespace BetterCms.Module.Users.Api
{
    /// <summary>
    /// API module descriptor.
    /// </summary>
    public class UsersApiModuleDescriptor : CmsModuleDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersApiModuleDescriptor" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public UsersApiModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of API module.
        /// </value>
        public override string Name
        {
            get
            {
                return "users-api";
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "An Users API module for Better CMS.";
            }
        }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public override int Order
        {
            get
            {
                return int.MaxValue - 50;
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UsersService>().As<IUsersService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<ValidateUserService>().As<IValidateUserService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RolesService>().As<IRolesService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            containerBuilder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope().PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
        }
    }
}
