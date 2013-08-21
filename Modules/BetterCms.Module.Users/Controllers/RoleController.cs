using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Users.Commands.Role.DeleteRole;
using BetterCms.Module.Users.Commands.Role.EditRole;
using BetterCms.Module.Users.Commands.Role.GetRoleForEdit;
using BetterCms.Module.Users.Commands.Role.GetRoles;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels.Role;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// Role management.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class RoleController : CmsControllerBase
    {
        /// <summary>
        /// An action to delete a given role.
        /// </summary>
        /// <param name="id">The role id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json with status.</returns>
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

        /// <summary>
        /// Creates the role view.
        /// </summary>
        /// <returns>Role create view.</returns>
        public ActionResult CreateRoleView()
        {
            var model = GetCommand<GetRoleForEditCommand>().ExecuteCommand(null);
            return PartialView("EditRoleView", model);
        }

        /// <summary>
        /// Edits the role view.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Role edit view.</returns>
        public ActionResult EditRoleView(string id)
        {
            var model = GetCommand<GetRoleForEditCommand>().ExecuteCommand(id.ToGuidOrDefault());
            return PartialView(model);
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json with status.</returns>
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

        /// <summary>
        /// Roles list for Site Settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Role list view.</returns>
        public ActionResult RolesListView(SearchableGridOptions request)
        {
            var roleList = GetCommand<GetRolesCommand>().ExecuteCommand(request);            
            var model = new SiteSettingRoleListViewModel(roleList, new SearchableGridOptions(), roleList.Count);
            return View(model);
        }
    }
}
