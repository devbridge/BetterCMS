// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JavaScriptModuleLinkTo.cs" company="Devbridge Group LLC">
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
using System.Collections;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Common.Logging;

using BetterModules.Core.Web.Modules;

using Microsoft.Web.Mvc;

namespace BetterCms.Core.Modules.Projections
{
    public class JavaScriptModuleLinkTo<TController> : IActionProjection where TController : Controller
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string linkName;

        private Expression<Action<TController>> urlExpression;

        private JsIncludeDescriptor descriptor;

        private bool fullUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="BetterCms.Core.Modules.Projections.JavaScriptModuleLinkTo{TController}" /> class.
        /// </summary>
        /// <param name="descriptor">The java script module.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="urlExpression">The URL expression.</param>
        /// <param name="fullUrl">if set to <c>true</c> renders full URL.</param>
        public JavaScriptModuleLinkTo(JsIncludeDescriptor descriptor, string linkName, Expression<Action<TController>> urlExpression, bool fullUrl = false)
        {
            this.fullUrl = fullUrl;
            this.descriptor = descriptor;
            this.urlExpression = urlExpression;
            this.linkName = linkName;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        public void Render(HtmlHelper html)
        {
            try
            {
                RouteValueDictionary routeValuesFromExpression = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(urlExpression);

                string url = HttpUtility.UrlDecode(html.BuildUrlFromExpression(urlExpression));
                if (fullUrl)
                {
                    var requestUrl =html.ViewContext.HttpContext.Request.Url;
                    if (requestUrl != null)
                    {
                        bool isCustomPort = requestUrl.Scheme == Uri.UriSchemeHttp && requestUrl.Port != 80 || requestUrl.Scheme == Uri.UriSchemeHttps && requestUrl.Port != 433;
                        url = string.Concat(requestUrl.Scheme, "://", requestUrl.Host, isCustomPort ? ":" + requestUrl.Port : string.Empty, url);
                    }
                }
                string link = string.Format("{0}.links.{1} = '{2}';", descriptor.FriendlyName, linkName, url);
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, urlExpression, descriptor);
            }
        }
    }
}
