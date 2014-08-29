using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.OpenGraphIntegration.Projections
{
    public class OpenGraphMetaDataProjection : HtmlElementProjection
    {
        private readonly string property;
        private readonly string content;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphMetaDataProjection" /> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="content">The content.</param>
        public OpenGraphMetaDataProjection(string property, string content)
            : base("meta", true)
        {
            this.property = string.Format("{0}{1}", OpenGrachIntegrationModuleConstants.OpenGraphPropertyPrefix, property);
            this.content = content;
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            controlRenderer.Attributes["property"] = property;
            controlRenderer.Attributes["content"] = content;
        }
    }
}