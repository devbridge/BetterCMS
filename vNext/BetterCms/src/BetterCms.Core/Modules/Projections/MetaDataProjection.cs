using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Renders meta data element (meta).
    /// </summary>
    public class MetaDataProjection : HtmlElementProjection
    {
        private readonly string name;
        private readonly string content;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaDataProjection" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public MetaDataProjection(string name, string content)
            : base("meta", true)
        {
            this.name = name;
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
            controlRenderer.Attributes["name"] = name;
            controlRenderer.Attributes["content"] = content;
        }
    }
}
