using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Core;
using BetterCms.Core.DataContracts;

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
            IList<IPage> pages;
            IList<IPage> queryablePages;
            using (var api =  CmsContext.CreateDataApi())
            {
                pages = api.Pages.GetPages();
                queryablePages = api.Pages.GetPagesQueryable().Take(2).ToList();
            }
            
            var count = pages.Count;
            var message = string.Format("Pages count: {0}", count);
            if (count > 0)
            {
                message = string.Format("{0}<br /> Pages titles: {1}", message, string.Join("; ", pages.Select(t => t.Title)));
            }

            // Queryable test
            count = queryablePages.Count;
            message = string.Format("{0}<br /><hr />Queryable pages count [Max 2]: {1}", message, count);
            if (count > 0)
            {
                message = string.Format("{0}<br /> Queryable pages titles, ordered by title [Max 2]: {1}", message, string.Join("; ", queryablePages.Select(t => t.Title)));
            }

            return Content(message);
        }
        
    }
}
