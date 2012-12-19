using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

using BetterCms.Core.Models;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Projection to support action projections inheritance.
    /// </summary>
    public class InheriteProjection : HtmlElementProjection
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="InheriteProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="childProjections">The child projections.</param>
        public InheriteProjection(string tag, IEnumerable<IPageActionProjection> childProjections)
            : base(tag)
        {
            ChildProjections = childProjections;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InheriteProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="childProjections">The child projections.</param>
        /// <param name="order">The order.</param>
        public InheriteProjection(string tag, IEnumerable<IPageActionProjection> childProjections, int order)
            : base(tag)
        {
            ChildProjections = childProjections;
            Order = order;
        }

        /// <summary>
        /// Gets or sets the child projections.
        /// </summary>
        /// <value>
        /// The child projections.
        /// </value>
        public IEnumerable<IPageActionProjection> ChildProjections { get; set; }

        /// <summary>
        /// Renders an action projection to given html output.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        public override void Render(IPage page, HtmlHelper html)
        {
            using (HtmlControlRenderer control = new HtmlControlRenderer(Tag))
            {
                OnPreRender(control, page, html);

                using (HtmlTextWriter writer = new HtmlTextWriter(html.ViewContext.Writer))
                {
                    control.RenderBeginTag(writer);
                    
                    if (ChildProjections != null)
                    {
                        foreach (var htmlElementProjection in ChildProjections.OrderBy(f => f.Order))
                        {
                            htmlElementProjection.Render(page, html);
                        }
                    }

                    control.RenderEndTag(writer);
                }
            }
        }
    }
}
