using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Renders relation element (link).
    /// </summary>
    public class RelationProjection : HtmlElementProjection
    {
        private readonly string relation;
        private readonly string type;
        private readonly string link;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationProjection" /> class.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="link">The link.</param>
        public RelationProjection(string relation, string link) : this(relation, link, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationProjection" /> class.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="link">The link.</param>
        /// <param name="type">The type.</param>
        public RelationProjection(string relation, string link, string type)
            : base("link", true)
        {
            this.relation = relation;
            this.type = type;
            this.link = link;
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            controlRenderer.Attributes["rel"] = relation;
            controlRenderer.Attributes["href"] = link;
            if (!string.IsNullOrWhiteSpace(type))
            {
                controlRenderer.Attributes["type"] = type;
            }
        }
    }
}
