// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthenticationHelper.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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