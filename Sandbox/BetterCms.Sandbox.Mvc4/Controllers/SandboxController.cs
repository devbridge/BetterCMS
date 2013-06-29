using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.Api.DataContracts;
using BetterCms.Events;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Api.DataContracts;
using BetterCms.Module.Pages.Models;
using BetterCms.Sandbox.Mvc4.Models;

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
        public ActionResult Login(string roles)
        {
            //            var roles = string.Join(",", Roles.GetRolesForUser(string.Empty));
            if (string.IsNullOrEmpty(roles))
            {
                roles = "Owner";
            }

            var authTicket = new FormsAuthenticationTicket(1, "Better CMS test user", DateTime.Now, DateTime.Now.AddMonths(1), true, roles);

            var cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            HttpContext.Response.Cookies.Add(cookie);

            return Redirect("/");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

            return Redirect("/");
        }

        public ActionResult TestNavigationApi()
        {

            var message = new StringBuilder("No sitemap data found!");

            return Content(message.ToString());
        }

        [AllowAnonymous]
        public ActionResult LoginJson(LoginViewModel login)
        {
            Login(string.Empty);

            return Json(new { Success = true });
        }

        [AllowAnonymous]
        public ActionResult LogoutJson(LoginViewModel login)
        {
            Logout();

            return Json(new { Success = true });
        }

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}