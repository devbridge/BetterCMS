using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.ViewModels.Role;

using NHibernate.Transform;

namespace BetterCms.Module.Users.Commands.Role.GetRoleForEdit
{
    public class GetRoleForEditCommand : CommandBase, ICommand<Guid?, EditRoleViewModel>
    {
        public EditRoleViewModel Execute(Guid? roleId)
        {
            return null;
            //            var permissionsList = GetAllPermissions();
            //            EditRoleViewModel roleModel;
            //            if (roleId == null)
            //            {
            //                roleModel = new EditRoleViewModel();
            //                roleModel.PermissionsList = permissionsList;
            //            }
            //            else
            //            {
            //                EditRoleViewModel roleAlias = null;
            //                Models.Role role = null;
            //
            //                roleModel = UnitOfWork.Session.QueryOver(() => role)
            //                              .Where(() => role.Id == roleId).SelectList(select => select
            //                                  .Select(() => role.Id).WithAlias(() => roleAlias.Id)
            //                                  .Select(() => role.Version).WithAlias(() => roleAlias.Version)
            //                                  .Select(() => role.Name).WithAlias(() => roleAlias.RoleName))
            //
            //                                  .TransformUsing(Transformers.AliasToBean<EditRoleViewModel>())
            //                                  .FutureValue<EditRoleViewModel>().Value;
            //
            //                RolePermission permissions = null;
            //                PermissionViewModel permissionAlias = null;
            //
            //                var rolePermissions =
            //                    UnitOfWork.Session.QueryOver(() => permissions)
            //                              .Where(() => permissions.Role.Id == roleId && !permissions.IsDeleted)
            //                              .SelectList(select => select
            //                                  .Select(() => permissions.Permission.Id).WithAlias(() => permissionAlias.Id))
            //                              .TransformUsing(Transformers.AliasToBean<PermissionViewModel>())
            //                              .List<PermissionViewModel>().Select(p => p.Id).ToArray();
            //
            //                permissionsList.Where(p => rolePermissions.Contains(p.Id)).ToList().ForEach(p => p.IsSelected = true);
            //
            //                roleModel.PermissionsList = permissionsList;
            //            }
            //            return roleModel; 
        }


//        private IList<PermissionViewModel> GetAllPermissions()
//        {
//            var premissions = Repository
//                .AsQueryable<Permission>()
//                .Select(t => new PermissionViewModel
//                {
//                    Id = t.Id,
//                    Name = t.Description
//                })
//                .ToList();
//
//            return premissions;
//        }
    }
}