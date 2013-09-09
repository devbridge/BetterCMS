using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Users.Provider;
using BetterCms.Sandbox.Mvc4.Helpers;
using BetterCms.Sandbox.Mvc4.Models;

using httpContext = System.Web.HttpContext;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class SandboxController : Controller
    {
        public ActionResult Content()
        {
            return Content("Hello from the web project controller.");
        }

        public ActionResult Hello()
        {
            return PartialView("Partial/Hello");
        }

        public ActionResult Widget05()
        {
            return PartialView("~/Views/Widgets/05.cshtml");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string roles)
        {
            if (Roles.Enabled && Roles.Provider is CmsRoleProvider)
            {
                var model = new LoginViewModel
                                {
                                    Identity =  User.Identity
                                };

                return View(model);
            }

            AuthenticationHelper.CreateTicket(!string.IsNullOrWhiteSpace(roles) ? roles.Split(',') : new[] { "Owner" });

            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (Membership.ValidateUser(login.UserName, login.Password))
            {
                var roles = Roles.GetRolesForUser(login.UserName);
                AuthenticationHelper.CreateTicket(roles, login.UserName);

                return Redirect("/");
            }

            return Login((string)null);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationHelper.Logout();
            
            return Redirect("/");
        }

        public ActionResult TestNavigationApi()
        {

            var message = new StringBuilder("No sitemap data found!");

            return Content(message.ToString());
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}