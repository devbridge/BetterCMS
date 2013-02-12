using System;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.UI;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Core.Models;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Represents base projection to render html elements.
    /// </summary>
    public class HtmlElementProjection : IPageActionProjection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElementProjection" /> class.
        /// </summary>
        /// <param name="tag">The tag key.</param>
        public HtmlElementProjection(string tag)
        {
            Tag = tag;
        }

        /// <summary>
        /// Gets or sets function to retrieve an id for html element.
        /// </summary>
        /// <value>
        /// A function to retrieve an id for html element.
        /// </value>
        public Func<IPage, string> Id { get; set; }

        /// <summary>
        /// Gets or sets function to retrieve html element CSS class.
        /// </summary>
        /// <value>
        /// A function to retrieve html element CSS class.
        /// </value>
        public Func<IPage, string> CssClass { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        /// <value>
        /// The tooltip.
        /// </value>
        public Func<IPage, string> Tooltip { get; set; } 

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets permission for rendering.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Func<IPage, IPrincipal, bool> IsVisible { get; set; }

        /// <summary>
        /// Gets html element tag name.
        /// </summary>
        protected string Tag { get; private set; }

        /// <summary>
        /// Renders a control.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="html">The HTML.</param>
        public virtual void Render(IPage page, IPrincipal principal, HtmlHelper html)
        {
            if (IsVisible != null && !IsVisible(page, principal))
            {
                return;
            }

            using (HtmlControlRenderer control = new HtmlControlRenderer(Tag))
            {
                OnPreRender(control, page, html);

                using (HtmlTextWriter writer = new HtmlTextWriter(html.ViewContext.Writer))
                {
                    control.RenderControl(writer);
                }
            }
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected virtual void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {            
            if (Id != null)
            {
                controlRenderer.Attributes["id"] = Id(page);
            }

            if (CssClass != null)
            {
                string css = controlRenderer.Attributes["class"];

                if (!string.IsNullOrEmpty(css))
                {
                    css = string.Concat(css, " ", CssClass(page));
                }
                else
                {
                    css = CssClass(page);
                }

                controlRenderer.Attributes.Add("class", css);
            }

            if (Tooltip != null)
            {
                string tooltip = Tooltip(page);
                controlRenderer.Attributes.Add("title", tooltip);
            }

            controlRenderer.Attributes.Add("data-bcms-order", Order.ToString());
        }   
    }
}
