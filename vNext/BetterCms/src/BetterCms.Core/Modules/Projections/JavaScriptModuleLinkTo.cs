using System;
using System.Linq.Expressions;
using BetterModules.Core.Web.Mvc.Extensions;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Framework.Logging;

namespace BetterCms.Core.Modules.Projections
{
    public class JavaScriptModuleLinkTo<TController> : IActionUrlProjection where TController : Controller
    {
        private readonly ILogger logger;

        private readonly IControllerExtensions controllerExtensions;

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
        /// <param name="controllerExtensions">Controller extensions</param>
        /// <param name="fullUrl">if set to <c>true</c> renders full URL.</param>
        /// <param name="loggerFactory">The logger factory</param>
        public JavaScriptModuleLinkTo(JsIncludeDescriptor descriptor, string linkName, Expression<Action<TController>> urlExpression, 
            ILoggerFactory loggerFactory, IControllerExtensions controllerExtensions, bool fullUrl = false)
        {
            this.fullUrl = fullUrl;
            this.descriptor = descriptor;
            this.urlExpression = urlExpression;
            this.linkName = linkName;
            logger = loggerFactory.CreateLogger<JavaScriptModuleLinkTo<TController>>();
            this.controllerExtensions = controllerExtensions;
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        public void Render(IHtmlHelper html, IUrlHelper url)
        {
            try
            {
                var actionName = ((MethodCallExpression) urlExpression.Body).Method.Name;
                var controllerName = controllerExtensions.GetControllerName(typeof (TController));
                string actionLink = url.Action(actionName, controllerName);
                if (fullUrl)
                {
                    var request = html.ViewContext.HttpContext.Request;
                    if (request != null)
                    {
                        //bool isCustomPort = request.Scheme == Uri.UriSchemeHttp && request. != 80 || request.Scheme == Uri.UriSchemeHttps && request.Port != 433;
                        actionLink = string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), actionLink);
                    }
                }
                string link = $"{descriptor.FriendlyName}.links.{linkName} = '{actionLink}';";
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, urlExpression, descriptor);
            }
        }
    }
}
