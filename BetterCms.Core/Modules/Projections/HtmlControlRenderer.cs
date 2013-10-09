using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BetterCms.Core.Modules.Projections
{
    public class HtmlControlRenderer : HtmlGenericControl
    {
        /// <summary>
        /// Determines, if html tag is self closing
        /// </summary>
        private readonly bool isSelfClosing;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlControlRenderer" /> class.
        /// </summary>
        /// <param name="tag">The name of the element for which this instance of the class is created.</param>
        /// <param name="isSelfClosing">if set to <c>true</c> html tag is self closing.</param>
        public HtmlControlRenderer(string tag, bool isSelfClosing = false)
            : base(tag)
        {
            this.isSelfClosing = isSelfClosing;
        }

        /// <summary>
        /// Renders the closing tag for the <see cref="T:System.Web.UI.HtmlControls.HtmlContainerControl" /> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the rendered content.</param>
        internal new void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
        }

        /// <summary>
        /// Renders the opening HTML tag of the control into the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the rendered content.</param>
        internal new void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
        }

        /// <summary>
        /// Renders the <see cref="T:System.Web.UI.HtmlControls.HtmlContainerControl" /> control to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the <see cref="T:System.Web.UI.HtmlControls.HtmlContainerControl" /> content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (isSelfClosing)
            {
                writer.Write(HtmlTextWriter.TagLeftChar + TagName);
                Attributes.Render(writer);
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                return;
            }

            base.Render(writer);
        }
    }
}
