using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
using BetterCms.Module.Users.Commands.User;
using BetterCms.Module.Users.Commands.User.GetUser;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels;

namespace BetterCms.Module.Users.Controllers
{
    public class UserController : CmsControllerBase
    {
        //
        // GET: /User/

        public ActionResult Index(SearchableGridOptions request)
        {
            
            IList<UserViewModel> user = new List<UserViewModel>();
            user.Add(new UserViewModel(){Name = "test"});
            user.Add(new UserViewModel() { Name = "test1" });
            var model = new SearchableGridViewModel<UserViewModel>(user, new SearchableGridOptions(), 0);
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
