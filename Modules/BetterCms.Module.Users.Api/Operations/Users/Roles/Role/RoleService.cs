// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Api.Operations.Users.Roles.Role;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Users.Api.Operations.Users.Roles.Role
{
    public class RoleService : Service, IRoleService
    {
        private readonly IRepository repository;
        
        private readonly IUnitOfWork unitOfWork;

        private readonly Services.IRoleService roleService;

        public RoleService(IRepository repository, Services.IRoleService roleService, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.roleService = roleService;
        }

        public GetRoleResponse Get(GetRoleRequest request)
        {
            var query = repository
                .AsQueryable<Models.Role>();

            if (request.RoleId.HasValue)
            {
                query = query.Where(role => role.Id == request.RoleId);
            }
            else
            {
                query = query.Where(role => role.Name == request.RoleName);
            }
            
            var model = query
                .Select(role => new RoleModel
                    {
                        Id = role.Id,
                        Version = role.Version,
                        CreatedBy = role.CreatedByUser,
                        CreatedOn = role.CreatedOn,
                        LastModifiedBy = role.ModifiedByUser,
                        LastModifiedOn = role.ModifiedOn,

                        Name = role.Name,
                        Description = role.Description,
                        IsSystematic = role.IsSystematic
                    })
                .FirstOne();

            return new GetRoleResponse { Data = model };
        }

        public PutRoleResponse Put(PutRoleRequest request)
        {
            bool isNew;
            unitOfWork.BeginTransaction();
            var role = roleService.SaveRole(request.Id ?? Guid.Empty, request.Data.Version, request.Data.Name, request.Data.Description, out isNew, true);
            unitOfWork.Commit();

            if (isNew)
            {
                Events.UserEvents.Instance.OnRoleCreated(role);
            }
            else
            {
                Events.UserEvents.Instance.OnRoleUpdated(role);
            }

            return new PutRoleResponse { Data = role.Id };
        }

        public DeleteRoleResponse Delete(DeleteRoleRequest request)
        {
            unitOfWork.BeginTransaction();
            var role = roleService.DeleteRole(request.Id, request.Data.Version, true);
            unitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnRoleDeleted(role);

            return new DeleteRoleResponse { Data = true };
        }
    }
}