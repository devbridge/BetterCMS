using System;
using System.Web;
using System.Web.Security;

namespace BetterCms.Sandbox.Mvc4.Helpers
{
    public static class AuthenticationHelper
    {
        public static void CreateTicket(string[] roles, string userName = "Better CMS test user")
        {
            var authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMonths(1), true, string.Join(",", roles));

            var cookieContents = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
            {
                Expires = authTicket.Expiration,
                Path = FormsAuthentication.FormsCookiePath
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }

        public static void Auth(HttpApplication http)
        {
            //var authCookie = http.Request.Cookies[FormsAuthentication.FormsCookieName];

            //if (authCookie != null)
            //{
            //    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    if (authTicket != null)
            //    {
            //        var identity = new FormsIdentity(authTicket);
            //        var principal = Roles.Enabled ? new RolePrincipal("BetterCmsRoleProvider", identity) : new RolePrincipal(identity, roleCokie.Value);
            //        http.Context.User = principal;
            //    }
            //}
        }
    }
}