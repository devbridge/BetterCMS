// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BcmsAuthorizeAttribute.cs" company="Devbridge Group LLC">
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
using System.Net;
using System.Web;
using System.Web.Mvc;

using Autofac;

using BetterCms.Core.Services;

using BetterModules.Core.Web.Dependencies;

namespace BetterCms.Core.Security
{
    /// <summary>
    /// Authorization attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BcmsAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BcmsAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="roles">The roles.</param>
        public BcmsAuthorizeAttribute(params string[] roles)
        {
            Roles = string.Join(",", roles);
        }

        /// <summary>
        /// When overridden, provides an entry point for custom authorization checks.
        /// </summary>
        /// <param name="httpContext">The HTTP context, which encapsulates all HTTP-specific information about an individual HTTP request.</param>
        /// <returns>
        /// true if the user is authorized; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">if httpContext is null.</exception>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var container = PerWebRequestContainerProvider.GetLifetimeScope(httpContext);
            if (container != null && container.IsRegistered<ISecurityService>())
            {
                var security = container.Resolve<ISecurityService>();
                return security.IsAuthorized(httpContext.User, Roles);
            }

            return base.AuthorizeCore(httpContext);
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
