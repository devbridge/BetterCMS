using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Users.Provider;
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

            CreateTicket(!string.IsNullOrWhiteSpace(roles) ? roles.Split(',') : new[] { "Owner" });

            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (Membership.ValidateUser(login.UserName, login.Password))
            {
                var roles = Roles.GetRolesForUser(login.UserName);
                CreateTicket(roles, login.UserName);

                return Redirect("/");
            }

            return Login((string)null);
        }

        private void CreateTicket(string[] roles, string userName = "Better CMS test user")
        {
            var authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMonths(1), true, string.Join(",", roles));

            var cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            HttpContext.Response.Cookies.Add(cookie);
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

        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}