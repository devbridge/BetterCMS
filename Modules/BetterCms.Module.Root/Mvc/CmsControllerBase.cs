// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsControllerBase.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;
using System.Web.Security;

using Autofac;

using BetterCms.Core.Services;
using BetterCms.Core.Web;

using BetterModules.Core.Web.Dependencies;
using BetterModules.Core.Web.Mvc;

namespace BetterCms.Module.Root.Mvc
{    
    /// <summary>
    /// Custom controller base.
    /// </summary>    
    public abstract class CmsControllerBase : CoreControllerBase
    {
        /// <summary>
        /// The HTTP context tool
        /// </summary>
        private HttpContextTool httpContextTool;

        /// <summary>
        /// Gets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        public ISecurityService SecurityService
        {
            get
            {
                var container = PerWebRequestContainerProvider.GetLifetimeScope(HttpContext);
                if (container != null && container.IsRegistered<ISecurityService>())
                {
                    return container.Resolve<ISecurityService>();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP context helper tool.
        /// </summary>
        /// <value>
        /// The HTTP context helper tool.
        /// </value>
        public HttpContextTool Http
        {
            get
            {
                if (httpContextTool == null && HttpContext != null)
                {
                    httpContextTool = new HttpContextTool(HttpContext);
                }
                return httpContextTool;
            }
            set { httpContextTool = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsControllerBase" /> class.
        /// </summary>
        protected CmsControllerBase()
        {     
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
        }

        /// <summary>
        /// Creates a <see cref="T:System.Web.Mvc.ViewResult" /> object using the view name and master-page name that renders a view to the response.
        /// </summary>
        /// <param name="model">View model object.</param>
        /// <param name="contentType">Response content type.</param>
        /// <returns>
        /// The view result.
        /// </returns>
        [NonAction]
        public virtual ActionResult View(object model, string contentType)
        {
            Response.ContentType = contentType;

            return View(model);
        }

        /// <summary>
        /// Signs out user.
        /// </summary>
        [NonAction]
        protected virtual ActionResult SignOutUserIfAuthenticated()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (FormsAuthentication.IsEnabled)
                {
                    FormsAuthentication.SignOut();
                }

                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                HttpCookie roleCokie = Roles.Enabled ? Request.Cookies[Roles.CookieName] : null;

                if (authCookie != null)
                {
                    Response.Cookies.Add(
                        new HttpCookie(authCookie.Name)
                            {
                                Expires = DateTime.Now.AddDays(-10)
                            });
                }

                if (roleCokie != null)
                {
                    Response.Cookies.Add(
                        new HttpCookie(roleCokie.Name)
                            {
                                Expires = DateTime.Now.AddDays(-10)
                            });
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }
    }
}
