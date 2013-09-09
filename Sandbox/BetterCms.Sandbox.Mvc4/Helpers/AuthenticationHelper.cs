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
    }
}