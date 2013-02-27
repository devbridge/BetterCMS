using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Core;
using BetterCms.Module.MediaManager;
using BetterCms.Module.MediaManager.Models;

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
            IList<MediaFolder> folders;
            using (var api = CmsContext.CreateDataApi<MediaManagerApiContext>())
            {
                folders = api.Medias.GetFolders(MediaType.Image);
            }

            var count = folders.Count;
            var message = string.Format("Image folders count: {0}", count);
            if (count > 0)
            {
                message = string.Format("{0}<br /> Image folders titles: {1}", message, string.Join("; ", folders.Select(t => t.Title)));
            }

            return Content(message);
        }
        
    }
}
