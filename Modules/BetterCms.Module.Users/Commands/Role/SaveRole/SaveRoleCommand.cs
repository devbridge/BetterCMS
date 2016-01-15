// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveRoleCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Role;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Role.SaveRole
{
    /// <summary>
    /// A command to save role item.
    /// </summary>
    public class SaveRoleCommand : CommandBase, ICommand<RoleViewModel, SaveRoleCommandResponse>
    {
        private readonly IRoleService roleService;

        public SaveRoleCommand(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response with id and version
        /// </returns>
        public SaveRoleCommandResponse Execute(RoleViewModel request)
        {
            Models.Role role;
            if (request.Id.HasDefaultValue())
            {
                role = roleService.CreateRole(request.Name, request.Description);
                UnitOfWork.Commit();
                Events.UserEvents.Instance.OnRoleCreated(role);
            }
            else
            {
                role = roleService.UpdateRole(request.Id, request.Version, request.Name, request.Description);
                UnitOfWork.Commit();
                Events.UserEvents.Instance.OnRoleUpdated(role);
            }

            return new SaveRoleCommandResponse { Id = role.Id, Name = role.Name, Version = role.Version };
        }
    }
}