using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Api;

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
        public ActionResult Login()
        {
            var authTicket = new FormsAuthenticationTicket(1, "BetterCMS test user", DateTime.Now, DateTime.Now.AddMonths(1), true, "User,Admin");

            string cookieContents = FormsAuthentication.Encrypt(authTicket);
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

        public ActionResult TestApi()
        {
            var tags = CmsContext.Api.Tags.GetTags();
            
            var count = tags.Count;
            var message = string.Format("Tags count: {0}", count);
            if (count > 0)
            {
                message = string.Format("{0}<br /> Tag names: {1}", message, string.Join("; ", tags.Select(t => t.Name)));
            }
            return Content(message);
        }
    }
}
