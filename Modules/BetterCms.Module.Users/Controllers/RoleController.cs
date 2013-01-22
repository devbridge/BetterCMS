using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Users.ViewModels.Role;

namespace BetterCms.Module.Users.Controllers
{
    public class RoleController : CmsControllerBase
    {
        public ActionResult EditRoleView(Guid? id)
        {
            var model = new EditRoleViewModel();
            var premissionList = new List<Premission>();
            for (int i = 0; i < 6; i++)
            {
                premissionList.Add(new Premission() { Name = "Owner" });
                premissionList.Add(new Premission() { Name = "Administrator" });
                premissionList.Add(new Premission() { Name = "Publisher" });
                premissionList.Add(new Premission() { Name = "Content Creator" });
                premissionList.Add(new Premission() { Name = "Content Editor" });
            }
            model.PremissionsList = premissionList;

            return PartialView(model);
        }

        public ActionResult RolesListView(SearchableGridOptions request)
        {
            var roleList = new List<RoleItemViewModel>();
            roleList.Add(new RoleItemViewModel(){RoleName = "admin"});
            roleList.Add(new RoleItemViewModel() { RoleName = "user" });
            var model = new SiteSettingRoleListViewModel(roleList, new SearchableGridOptions(), roleList.Count);
            return View(model);
        }

    }
}
