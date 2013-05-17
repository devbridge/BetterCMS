using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Api;
using BetterCms.Core;
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
            /*PagesApiContext.Events.PageCreated += EventsOnPageCreated ;

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

            return Content(message);*/

            ServerControlWidget result;

            
            using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                using (var transaction = new System.Transactions.TransactionScope())
                {
                    // pagesApi.CreateLayout(/* parameters */);
                    // result = pagesApi.CreateLayoutRegion(new Guid("8E954FBD-1550-4EEC-B730-A1C100EC25D3"), region);
                    // pagesApi.CreateHtmlContentWidget(/* parameters */);

                    string name = "API Server Widget";
                    string path = "~/Views/Widgets/01.cshtml";
                    var categoryId = new Guid("A4662051-CE31-431F-8B56-A1B200F63C21");
                    var previewUrl = "http://www.yahoo.com";

                    /*string css = "";
                    string javaScript = "";
                    string html = "<div class='aaa'>HTML CONTENT GOES HERE</div>";
                    Guid categoryId = new Guid("A4662051-CE31-431F-8B56-A1B200F63C21");*/

                    result = pagesApi.CreateServerControlWidget(name, path, categoryId, previewUrl);

                    pagesApi.CreateContentOption(result.Id, "Option 1", "def Value 1");
                    pagesApi.CreateContentOption(result.Id, "Option 2", "def Value 2");

                    transaction.Complete();
                }
            }
           
            

            return Content(result.Id.ToString());
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
    }
}
