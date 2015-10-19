using System;

using BetterModules.Core.Web.Modules;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Framework.Logging;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Java script module resource initialization renderer.
    /// </summary>
    public class JavaScriptModuleGlobalization : IActionProjection
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Resource name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Function to retrieve resource in current culture.
        /// </summary>
        private readonly Func<string> resource;

        /// <summary>
        /// Resources module.
        /// </summary>
        private readonly JsIncludeDescriptor jsModuleInclude;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptModuleGlobalization" /> class.
        /// </summary>
        /// <param name="jsModuleInclude">The java script module.</param>
        /// <param name="name">The name.</param>
        /// <param name="resource">A function to retrieve resource in current culture.</param>
        public JavaScriptModuleGlobalization(JsIncludeDescriptor jsModuleInclude, string name, Func<string> resource, ILoggerFactory loggerFactory)
        {
            this.jsModuleInclude = jsModuleInclude;
            this.name = name;
            this.resource = resource;
            logger = loggerFactory.CreateLogger<JavaScriptModuleGlobalization>();
        }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Renders java script module resource initialization. 
        /// </summary>
        /// <param name="html">Html helper.</param>
        public void Render(IHtmlHelper html)
        {
            try
            {
                var resourceObject = resource();
                if (resourceObject != null)
                {
                    string globalization =
                        $"{jsModuleInclude.FriendlyName}.globalization.{name} = '{resourceObject.Replace("'", "\\'")}';";
                    html.ViewContext.Writer.WriteLine(globalization);
                }
                else
                {
                    logger.LogWarning("Resource object not found to globalize {0}.{1} from resource {2}.", jsModuleInclude, name, resource);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning("Failed to render globalization for {0}.{1} from resource {2}.", ex, jsModuleInclude, name, jsModuleInclude);
            }
        }        
    }
}
