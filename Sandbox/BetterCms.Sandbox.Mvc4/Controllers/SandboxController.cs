using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Api.Events;
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
        public ActionResult Login()
        {
            var roles = string.Join(",", Roles.GetRolesForUser(string.Empty));
            var authTicket = new FormsAuthenticationTicket(1, "BetterCMS test user", DateTime.Now, DateTime.Now.AddMonths(1), true, roles);

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
            PagesApiContext.Events.PageCreated += EventsOnPageCreated ;

            PagesApiContext.Events.OnPageCreated(new PageProperties());

            ApiContext.Events.HostStart += Core_HostStart;

            IList<MediaFolder> folders;
            using (var mediaApi = CmsContext.CreateApiContextOf<MediaManagerApiContext>())
            {                
                folders = mediaApi.GetFolders(MediaType.Image);
            }

            var count = folders.Count;
            var message = string.Format("Image folders count: {0}", count);

            if (count > 0)
            {
                message = string.Format("{0}<br /> Image folders titles: {1}", message, string.Join("; ", folders.Select(t => t.Title)));
            }

            return Content(message);
        }

        void Core_HostStart(SingleItemEventArgs<Core.Environment.Host.ICmsHost> args)
        {
            throw new NotImplementedException();
        }

        private void EventsOnPageCreated(SingleItemEventArgs<PageProperties> args)
        {
            
        }

        public ActionResult TestNavigationApi()
        {
            
            var message = new StringBuilder("No sitemap data found!");                     

            return Content(message.ToString());
        }        

        [AllowAnonymous]
        public ActionResult LoginJson(LoginViewModel login)
        {
            Login();

            return Json(new { Success = true });
        }

        [AllowAnonymous]
        public ActionResult LogoutJson(LoginViewModel login)
        {
            Logout();

            return Json(new { Success = true });
        }
    }
}
