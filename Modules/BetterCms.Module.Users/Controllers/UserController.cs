using System.Web.Mvc;

using BetterCms.Core.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Users.Commands.User;
using BetterCms.Module.Users.Commands.User.DeleteUser;
using BetterCms.Module.Users.Commands.User.GetUser;
using BetterCms.Module.Users.Commands.User.GetUsersList;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Controllers
{
    public class UserController : CmsControllerBase
    {
        public ActionResult Index(SearchableGridOptions request)
        {
            var users = GetCommand<GetUsersCommand>().ExecuteCommand(request);
            var model = new SearchableGridViewModel<UserItemViewModel>(users, new SearchableGridOptions(), users.Count);
            return View(model);
        }

        public ActionResult EditUser(string id)
        {
            var model = GetCommand<GetUserCommand>().ExecuteCommand(id.ToGuidOrDefault());
            return PartialView("EditUserView", model);
        }

        public ActionResult SaveUser(EditUserViewModel model)
        {
            var response = GetCommand<SaveUserCommand>().ExecuteCommand(model);
            if (response != null)
            {
                Messages.AddSuccess(UsersGlobalization.SaveUser_CreatedSuccessfully_Message);
                return Json(new WireJson { Success = true, Data = response });
            }
            return Json(new WireJson { Success = false });
        }

        public ActionResult DeleteUser(string id, string version)
        {
            bool success = GetCommand<DeleteUserCommand>().ExecuteCommand(
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
