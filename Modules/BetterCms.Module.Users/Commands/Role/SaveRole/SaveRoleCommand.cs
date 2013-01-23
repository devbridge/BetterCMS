using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.ViewModels.Role;
using BetterCms.Module.Users.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.Role.EditRole
{
    public class SaveRoleCommand : CommandBase, ICommand<EditRoleViewModel, SaveRoleResponse>
    {
        public SaveRoleResponse Execute(EditRoleViewModel request)
        {
            UnitOfWork.BeginTransaction();
   
            var role = !request.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Models.Role>()
                                           .Where(f => f.Id == request.Id)
                                           .ToList()
                                           .FirstOrDefault()
                               : new Models.Role();

            if (role == null)
            {
                role = new Models.Role();
            }

            role.Name = request.RoleName;
            role.Version = request.Version;

            //add new
         /*   foreach (var permission in request.PermissionsList)
            {
                if (permission.IsSelected)
                {
                    var rolePermission = new RolePermissions();
                    rolePermission.Role = new Models.Role();
                    rolePermission.Permission= new Permission();

                    rolePermission.Role.Id = request.Id;
                    rolePermission.Role.Name = request.RoleName;
                    rolePermission.Permission.Id = permission.Id;
                    rolePermission.Permission.Name = permission.Name;

                    Repository.Save(rolePermission);
                }
            }*/

            Repository.Save(role);
            UnitOfWork.Commit();

            return new SaveRoleResponse() { Id = role.Id, RoleName = role.Name, Version = role.Version };

        }

        private IList<RolePermissions> GetRolePermissions(Guid roleId)
        {
            var rolePermissions = UnitOfWork.Session
                    .Query<RolePermissions>()
                    .Where(r => !r.IsDeleted && r.Role.Id == roleId)
                    .ToList(); 

            return rolePermissions;
        } 
    }
}