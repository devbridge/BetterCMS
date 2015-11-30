// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetRoleRequestValidator.cs" company="Devbridge Group LLC">
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
using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Users.Roles.Role
{
    public class GetRoleRequestValidator : AbstractValidator<GetRoleRequest>
    {
        public GetRoleRequestValidator()
        {
            RuleFor(request => request.RoleId).Must(RoleNameMustBeNullIfRoleIdProvided).WithMessage("A RoleName field must be null if RoleId is provided.");
            RuleFor(request => request.RoleName).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A RoleId or RoleName should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetRoleRequest getRoleRequest, string roleName)
        {
            return getRoleRequest.RoleId != null || !string.IsNullOrEmpty(getRoleRequest.RoleName);
        }

        private bool RoleNameMustBeNullIfRoleIdProvided(GetRoleRequest getRoleRequest, System.Guid? roleId)
        {
            return roleId != null && string.IsNullOrEmpty(getRoleRequest.RoleName) ||
                   roleId == null && !string.IsNullOrEmpty(getRoleRequest.RoleName);
        }
    }
}
