using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BetterCms.Api.Tests.Controllers
{
    public class AuthenticationController : Controller
    {
#if DEBUG
        [AllowAnonymous]
        public ActionResult Login(string roles)
        {
            if (string.IsNullOrEmpty(roles))
            {
                roles = "Owner";
            }

            var authTicket = new FormsAuthenticationTicket(1, "Better CMS test user", DateTime.Now, DateTime.Now.AddMonths(1), true, roles);

            var cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents) {
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
#endif
    }
}