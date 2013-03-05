using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Core;
using BetterCms.Module.MediaManager;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Navigation;

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
            using (var mediaApi = CmsContext.CreateApiContextOf<MediaManagerApiContext>())
            {
                using (var navigationApi = CmsContext.CreateApiContextOf<NavigationApiContext>(mediaApi))
                {
                    folders = mediaApi.Medias.GetFolders(MediaType.Image);
                }
            }

            var count = folders.Count;
            var message = string.Format("Image folders count: {0}", count);
            if (count > 0)
            {
                message = string.Format("{0}<br /> Image folders titles: {1}", message, string.Join("; ", folders.Select(t => t.Title)));
            }

            return Content(message);
        }

        public ActionResult TestNavigationApi()
        {
            var message = new StringBuilder("No sitemap data found!");            

            using (var api = CmsContext.CreateApiContextOf<NavigationApiContext>())
            {              
                var sitemap = api.Sitemap.GetSitemapTree();
                message.Clear();
                message.AppendFormat("Sitemap contains {0} root nodes:", sitemap.Count);
                message.Append("<br><br>GetSitemapTree():<br>");
                message.Append(string.Join("<br> ", sitemap.Select(n => string.Format("<b>{0}</b>: {1}", n.Title, n.Url))));

                if (sitemap.Count > 0)
                {
                    var node = api.Sitemap.GetNode(sitemap[0].Id);
                    message.Append("<br><br>GetNode(id):<br>");
                    message.Append(node.Title);

                    var letter = sitemap[0].Title.ToArray()[0];
                    var nodes = api.Sitemap.GetNodes(n => n.Title.Contains(letter.ToString(CultureInfo.InvariantCulture)));
                    message.AppendFormat("<br><br>GetNodes(where title contains '{0}'):<br>", letter);
                    message.AppendFormat("{0} nodes found.", nodes.Count);
                }
            }

            return Content(message.ToString());
        }
        
    }
}
