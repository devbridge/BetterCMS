using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using BetterCms.Module.Users.Commands.User;
using BetterCms.Module.Users.Commands.User.DeleteUser;
using BetterCms.Module.Users.Commands.User.GetUser;
using BetterCms.Module.Users.Commands.User.GetUsersList;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User management.
    /// </summary>
    [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class UserController : CmsControllerBase
    {
        /// <summary>
        /// User list for Site Settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>User list view.</returns>
        public ActionResult Index(SearchableGridOptions request)
        {
            request.SetDefaultPaging();
            var model = GetCommand<GetUsersCommand>().ExecuteCommand(request);
            return View(model);
        }

        /// <summary>
        /// Edits the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>User edit view.</returns>
        public ActionResult EditUser(string id)
        {
            var model = GetCommand<GetUserCommand>().ExecuteCommand(id.ToGuidOrDefault());
            return PartialView("EditUserView", model);
        }

        /// <summary>
        /// Saves the user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Json status result.</returns>
        public ActionResult SaveUser(EditUserViewModel model)
        {
            var response = GetCommand<SaveUserCommand>().ExecuteCommand(model);
            if (response != null)
            {
                if (!model.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(UsersGlobalization.SaveUser_CreatedSuccessfully_Message);
                }
                return WireJson(true, response);
            }

            return WireJson(false);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="version">The version.</param>
        /// <returns>Json status result.</returns>
        public ActionResult DeleteUser(string id, string version)
        {
            var success = GetCommand<DeleteUserCommand>().ExecuteCommand(
                new DeleteUserCommandRequest
                {
                    UserId = id.ToGuidOrDefault(),
                    Version = version.ToIntOrDefault()
                });

            if (success)
            {
                Messages.AddSuccess(UsersGlobalization.DeleteUser_DeletedSuccessfully_Message);
            }

            return Json(new WireJson(success));
        }
    }
}
