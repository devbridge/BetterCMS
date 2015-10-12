using System;

using BetterCms.Core.DataContracts;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Core.Modules.Projections
{
    public class LinkToNewTabProjection : HtmlElementProjection
    {
        public Func<IPage, string> InnerText { get; set; }

        public Func<IPage, string> LinkAddress { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkToNewTabProjection" /> class.
        /// </summary>
        public LinkToNewTabProjection()
            : base("a", false)
        {
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(TagBuilder builder, IPage page, HtmlHelper html)
        {
            base.OnPreRender(builder, page, html);

            builder.Attributes["href"] = LinkAddress(page);
            builder.Attributes["target"] = "_blank";
            builder.InnerHtml.Append(InnerText(page));
        }
    }
}
