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
using BetterCms.Core.Api.DataContracts;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Pages.Api.Events;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
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

        public ActionResult TestApi()
        {
            /*PagesApiContext.Events.PageCreated += EventsOnPageCreated;

            PagesApiContext.Events.OnPageCreated(new PageProperties());

            ApiContext.Events.HostStart += Core_HostStart;*/

            IList<LayoutRegion> results;
            using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                /*var request = new GetDataRequest<Layout>(3, 2, orderDescending:true, order:t =>t.Name);
                results = pagesApi.GetLayouts(request);*/

                var request = new GetDataRequest<LayoutRegion>(2, 2, orderDescending: true, order: t => t.Region.RegionIdentifier);
                results = pagesApi.GetLayoutRegions(new Guid("F68B8A99-E06F-4A8A-9C67-A1C500A2919F"), request);
            }

            var count = results.Count;
            var message = string.Format("Items count: {0}", count);

            if (count > 0)
            {
                message = string.Format("{0}<br /> Item titles: {1}", message, string.Join("; ", results.Select(t => t.Region.RegionIdentifier)));
            }

            return Content(message);
        }

        void Core_HostStart(SingleItemEventArgs<HttpApplication> args)
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