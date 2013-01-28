using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Users.Commands.Role.DeleteRole;
using BetterCms.Module.Users.Commands.Role.EditRole;
using BetterCms.Module.Users.Commands.Role.GetRoleForEdit;
using BetterCms.Module.Users.Commands.Role.GetRoles;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels.Role;

namespace BetterCms.Module.Users.Controllers
{
    public class RoleController : CmsControllerBase
    {
        /// <summary>
        /// An action to delete a given role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <param name="version">The version.</param>
        /// <returns>
        /// Json with status.
        /// </returns>
        [HttpPost]
        public ActionResult DeleteRole(string id, string version)
        {
            bool success = GetCommand<DeleteRoleCommand>().ExecuteCommand(
                new DeleteRoleCommandRequest
                {
                    RoleId = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(UsersGlobalization.DeleteRole_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }

        public ActionResult CreatRoleView()
        {
            var model = GetCommand<GetRoleForEditCommand>().ExecuteCommand(null);

            return PartialView("EditRoleView",model);
        }

        public ActionResult EditRoleView(string id)
        {
            var model = GetCommand<GetRoleForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult CreateRole(EditRoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                var response = GetCommand<SaveRoleCommand>().ExecuteCommand(model);
                if (response != null)
                {
                   if (model.Id.HasDefaultValue())
                    {
                                Messages.AddSuccess(UsersGlobalization.SaveRole_CreatedSuccessfully_Message);
                    }
                    return Json(new WireJson { Success = true, Data = response });
                }
            }

            return Json(new WireJson { Success = false });
        }

        

        public ActionResult RolesListView(SearchableGridOptions request)
        {
            var roleList = GetCommand<GetRolesCommand>().Execute(request);            
            var model = new SiteSettingRoleListViewModel(roleList, new SearchableGridOptions(), roleList.Count);
            return View(model);
        }

    }
}
