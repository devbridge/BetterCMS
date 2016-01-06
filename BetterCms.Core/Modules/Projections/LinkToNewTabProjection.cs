using System;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;

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
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            base.OnPreRender(controlRenderer, page, html);

            controlRenderer.Attributes["href"] = LinkAddress(page);
            controlRenderer.Attributes["target"] = "_blank";
            controlRenderer.InnerText = InnerText(page);
        }
    }
}
