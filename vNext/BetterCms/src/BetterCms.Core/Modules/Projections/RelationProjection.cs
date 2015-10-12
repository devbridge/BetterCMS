using BetterCms.Core.DataContracts;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

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
        /// <param name="builder">The html tag builder.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(TagBuilder builder, IPage page, HtmlHelper html)
        {
            builder.Attributes["rel"] = relation;
            builder.Attributes["href"] = link;
            if (!string.IsNullOrWhiteSpace(type))
            {
                builder.Attributes["type"] = type;
            }
        }
    }
}
