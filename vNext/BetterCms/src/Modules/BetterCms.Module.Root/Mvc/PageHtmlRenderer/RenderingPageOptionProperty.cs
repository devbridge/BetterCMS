using System.Text;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public class RenderingPageOptionProperty : RenderingOptionPropertyBase, IRenderingPageProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPageOptionProperty" /> class.
        /// </summary>
        public RenderingPageOptionProperty()
            : base(RenderingPageProperties.PageOption)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        /// <returns>HTML with replaced model values</returns>
        public StringBuilder GetReplacedHtml(StringBuilder stringBuilder, RenderPageViewModel model)
        {
            return GetReplacedHtml(stringBuilder, model.Options);
        }
    }
}