using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Users.Commands.User;
using BetterCms.Module.Users.Commands.User.GetUser;
using BetterCms.Module.Users.Commands.User.GetUsersList;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Controllers
{
    public class UserController : CmsControllerBase
    {
        //
        // GET: /User/

        public ActionResult Index(SearchableGridOptions request)
        {
            var users = GetCommand<GetUsersCommand>().Execute(request);
            var model = new SearchableGridViewModel<UserItemViewModel>(users, new SearchableGridOptions(), users.Count);
            return View(model);
        }

        public ActionResult EditUser(string id)
        {
            var model = GetCommand<GetUserCommand>().Execute(id.ToGuidOrDefault());
            return PartialView("EditUserView", model);
        }

        public ActionResult SaveUser(EditUserViewModel model)
        {
            var response = GetCommand<SaveUserCommand>().Execute(model);
            if (response != null)
            {
                if (response.Id.HasDefaultValue())
                {
                    Messages.AddSuccess(UsersGlobalization.SaveRole_CreatedSuccessfully_Message);
                }
                return Json(new WireJson { Success = true, Data = response });
            }
            return Json(new WireJson { Success = false });
        }

    }
}
