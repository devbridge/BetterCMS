using System;
using BetterModules.Core.Web.Modules;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Framework.Logging;

namespace BetterCms.Core.Modules.Projections
{
    public class JavaScriptModuleLink : IActionProjection
    {
        private readonly ILogger logger;

        private string linkName;

        private JsIncludeDescriptor descriptor;

        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleLink" /> class.
        /// </summary>
        /// <param name="descriptor">The js module include.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="path">The path.</param>
        public JavaScriptModuleLink(JsIncludeDescriptor descriptor, string linkName, string path, ILoggerFactory loggerFactory)
        {
            this.path = path;
            this.descriptor = descriptor;
            this.linkName = linkName;
            logger = loggerFactory.CreateLogger<JavaScriptModuleLink>();
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
                string link = $"{descriptor.FriendlyName}.links.{linkName} = '{path}';";
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, path, descriptor);
            }
        }
    }
}
