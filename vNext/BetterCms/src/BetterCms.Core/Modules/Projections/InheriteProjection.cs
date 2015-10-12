using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Services;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.Extensions.WebEncoders;
using Microsoft.Extensions.DependencyInjection;

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
        /// <param name="securityService"></param>
        /// <param name="html">The html helper.</param>
        /// <returns><c>true</c> on success, otherwise <c>false</c>.</returns>
        public override bool Render(IPage page, ISecurityService securityService, HtmlHelper html)
        {
            if (AccessRole != null && !securityService.IsAuthorized(AccessRole))
            {
                return false;
            }

            var builder = new TagBuilder(Tag)
            {
                TagRenderMode = TagRenderMode.StartTag
            };
            var encoder = html.ViewContext.HttpContext.ApplicationServices.GetService<IHtmlEncoder>();
            OnPreRender(builder, page, html);
            builder.WriteTo(html.ViewContext.Writer, encoder);
            //control.RenderBeginTag(writer);

            if (ChildProjections != null)
            {
                foreach (var htmlElementProjection in ChildProjections.OrderBy(f => f.Order))
                {
                    htmlElementProjection.Render(page, securityService, html);
                }
            }

            builder = new TagBuilder(Tag)
            {
                TagRenderMode = TagRenderMode.EndTag
            };
            builder.WriteTo(html.ViewContext.Writer, encoder);
            //control.RenderEndTag(writer);

            return true;
        }
    }
}
