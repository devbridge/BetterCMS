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
    /// <summary>
    /// A command to save role item.
    /// </summary>
    public class SaveRoleCommand : CommandBase, ICommand<EditRoleViewModel, SaveRoleResponse>
    {
        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="roleItem">The role item.</param>
        /// <returns>
        /// true if role saved successfully; false otherwise.
        /// </returns>
        public SaveRoleResponse Execute(EditRoleViewModel roleItem)
        {
            UnitOfWork.BeginTransaction();

            var role = !roleItem.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Models.Role>()
                                           .Where(f => f.Id == roleItem.Id)
                                           .ToList()
                                           .FirstOrDefault()
                               : new Models.Role();

            if (role == null)
            {
                role = new Models.Role();
            }

            role.Name = roleItem.RoleName;
            role.Version = roleItem.Version;

            //edits or remove permission
            var rolePermissions = GetRolePermissions(roleItem.Id);
            var requestPermissionList = roleItem.PermissionsList.Where(p => p.IsSelected).ToList(); 

            if (rolePermissions != null && rolePermissions.Any())
            {
                foreach (var rolePermission in rolePermissions)
                {
                    var requestPermision = requestPermissionList.Count != 0
                                               ? requestPermissionList.FirstOrDefault(f => f.Id == rolePermission.Permission.Id && f.IsSelected)
                                               : null;

                    if (requestPermision != null)
                    {
                        Repository.Save(rolePermission);
                    }
                    else
                    {
                        Repository.Delete(rolePermission);
                    }
                }
                
                foreach (var rolePermission in rolePermissions)
                {
                    requestPermissionList.RemoveAll(x => x.Id == rolePermission.Permission.Id);
                }
            }

            var permissions = GetPermissions(requestPermissionList);
            
            //add new permission
            foreach (var permission in permissions)
            {
                    var rolePermission = new RolePermissions(){Permission = permission, Role = role};
                    Repository.Save(rolePermission);
            }

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

        private IList<Permission> GetPermissions(IList<PermissionViewModel> rolePermissions)
        {
            var identifiers = rolePermissions.Select(r => r.Id).ToArray();

            var permission = UnitOfWork.Session
                    .Query<Permission>()
                    .Where(p => !p.IsDeleted && identifiers.Contains(p.Id))
                    .ToList();

            return permission;
        } 
    }
}