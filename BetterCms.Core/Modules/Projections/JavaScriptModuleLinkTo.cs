using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

using Common.Logging;

using Microsoft.Web.Mvc;

namespace BetterCms.Core.Modules.JsModule
{
    public class JavaScriptModuleLinkTo<TController> : IActionProjection where TController : Controller
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string linkName;

        private Expression<Action<TController>> urlExpression;

        private JavaScriptModuleDescriptor javaScriptModule;

        private bool fullUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleLinkTo{TController}" /> class.
        /// </summary>
        /// <param name="javaScriptModule">The java script module.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="urlExpression">The URL expression.</param>
        /// <param name="fullUrl">if set to <c>true</c> renders full URL.</param>
        public JavaScriptModuleLinkTo(JavaScriptModuleDescriptor javaScriptModule, string linkName, Expression<Action<TController>> urlExpression, bool fullUrl = false)
        {
            this.fullUrl = fullUrl;
            this.javaScriptModule = javaScriptModule;
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
                string link = string.Format("{0}.links.{1} = '{2}';", javaScriptModule.FriendlyName, linkName, url);
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, urlExpression, javaScriptModule);
            }
        }
    }
}
