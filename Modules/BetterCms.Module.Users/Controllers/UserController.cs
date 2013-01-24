using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;
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

        public ActionResult EditUser()
        {
            var model = new EditUserViewModel();
            var roles = new List<LookupKeyValue>();
            roles.Add(new LookupKeyValue() { Key = "0", Value = "... Select role ..." });
            roles.Add(new LookupKeyValue(){Key = "0", Value = "Administrator"});
            roles.Add(new LookupKeyValue() { Key = "0", Value = "Content editor" });
            model.Roles = roles;
            return PartialView("EditUserView", model);
        }

    }
}
