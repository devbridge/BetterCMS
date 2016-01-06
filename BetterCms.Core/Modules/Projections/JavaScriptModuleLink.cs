using System;
using System.Web.Mvc;

using Common.Logging;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Core.Modules.Projections
{
    public class JavaScriptModuleLink : IActionProjection
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private string linkName;

        private JsIncludeDescriptor descriptor;

        private string path;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleLink" /> class.
        /// </summary>
        /// <param name="descriptor">The js module include.</param>
        /// <param name="linkName">Name of the link.</param>
        /// <param name="path">The path.</param>
        public JavaScriptModuleLink(JsIncludeDescriptor descriptor, string linkName, string path)
        {
            this.path = path;
            this.descriptor = descriptor;
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
                string link = string.Format("{0}.links.{1} = '{2}';", descriptor.FriendlyName, linkName, path);
                html.ViewContext.Writer.WriteLine(link);
            }
            catch (Exception ex)
            {
                Log.WarnFormat("Failed to render link '{0}' from expression {1} for java script module {2}.", ex, linkName, path, descriptor);
            }
        }
    }
}
