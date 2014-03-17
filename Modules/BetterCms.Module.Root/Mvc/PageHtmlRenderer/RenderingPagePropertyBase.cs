using System.Text;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public abstract class RenderingPagePropertyBase : RenderingPropertyBase, IRenderingPageProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderingPagePropertyBase" /> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public RenderingPagePropertyBase(string identifier)
            : base(identifier)
        {
        }

        /// <summary>
        /// Gets the replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        /// <returns>HTML with replaced model values</returns>
        public abstract StringBuilder GetReplacedHtml(StringBuilder stringBuilder, ViewModels.Cms.RenderPageViewModel model);
    }
}