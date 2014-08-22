using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Api;
using BetterCms.Module.Api.Operations.Pages.Sitemaps;
using BetterCms.Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
using BetterCms.Module.Users.Provider;
using BetterCms.Sandbox.Mvc4.Helpers;
using BetterCms.Sandbox.Mvc4.Models;

using httpContext = System.Web.HttpContext;

namespace BetterCms.Sandbox.Mvc4.Controllers
{
    public class SandboxController : Controller
    {
        private static Guid defaultSitemapId = new Guid("17ABFEE9-5AE6-470C-92E1-C2905036574B");

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

        public ActionResult TestRewrite(string url)
        {
            url = string.Concat("/", url.Trim('/'), "/");
            Server.TransferRequest(url);

            return new EmptyResult();
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

            AuthenticationHelper.CreateTicket(!string.IsNullOrWhiteSpace(roles) ? roles.Split(',') : new[] { "Owner" });

            return Redirect("/");
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (Membership.ValidateUser(login.UserName, login.Password))
            {
                var roles = Roles.GetRolesForUser(login.UserName);
                AuthenticationHelper.CreateTicket(roles, login.UserName);

                return Redirect("/");
            }

            return Login((string)null);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            AuthenticationHelper.Logout();
            
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


        public ActionResult SitemapMenu(string languageCode)
        {
            var renderIFrame = string.IsNullOrWhiteSpace(languageCode);
            var model = new SitemapMenuViewModel
                        {
                            ObsoleteMenuItems = new List<MenuItemViewModel>(),
                            MenuItems = new List<MenuItemViewModel>(),
                            RenderIFrame = renderIFrame,
                            LanguageCode = languageCode
                        };

            using (var api = ApiFactory.Create())
            {
                var languageId = GetLanguageId(api, languageCode);
                var sitemapId = GetSitemapId(api);
                if (sitemapId.HasValue)
                {
                    var request = new Module.Api.Operations.Pages.Sitemap.Tree.GetSitemapTreeRequest { SitemapId = sitemapId.Value };
                    request.Data.LanguageId = languageId ?? new Guid();
                    var response = api.Pages.Sitemap.Tree.Get(request);
                    if (response.Data.Count > 0)
                    {
                        model.ObsoleteMenuItems = response.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url, IsPublished = mi.PageIsPublished }).ToList();
                    }

                    var request1 = new Module.Api.Operations.Pages.Sitemaps.Sitemap.Tree.GetSitemapTreeRequest { SitemapId = sitemapId.Value };
                    request1.Data.LanguageId = languageId ?? new Guid();
                    var response1 = api.Pages.SitemapNew.Tree.Get(request1);
                    if (response.Data.Count > 0)
                    {
                        model.MenuItems = response1.Data.Select(mi => new MenuItemViewModel { Caption = mi.Title, Url = mi.Url, IsPublished = mi.PageIsPublished }).ToList();
                    }
                }

                if (model.RenderIFrame)
                {
                    model.LanguageCodes = api.Root.Languages.Get(new GetLanguagesRequest()).Data.Items.Select(l => l.Code).ToList();
                }
            }

            return View("~/Views/SitemapMenu/Index.cshtml", model);
        }

        private Guid? GetLanguageId(IApiFacade api, string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                return null;
            }

            try
            {
                var request = new GetLanguageRequest { LanguageCode = languageCode };
                var response = api.Root.Language.Get(request);
                return response.Data.Id;
            }
            catch
            {
            }

            return Guid.Empty;
        }

        private Guid? GetSitemapId(IApiFacade api)
        {
            var allSitemaps = api.Pages.Sitemaps.Get(new GetSitemapsRequest());
            if (allSitemaps.Data.Items.Count > 0)
            {
                var sitemap = allSitemaps.Data.Items.FirstOrDefault(map => map.Id == defaultSitemapId) ?? allSitemaps.Data.Items.First();
                return sitemap.Id;
            }

            return null;
        }
    }
}