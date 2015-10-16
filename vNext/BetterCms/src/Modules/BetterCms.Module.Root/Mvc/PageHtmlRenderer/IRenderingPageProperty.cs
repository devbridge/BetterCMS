using System.Text;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    public interface IRenderingPageProperty : IRenderingProperty
    {
        /// <summary>
        /// Gets the string builder with replaced HTML.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The render page view model.</param>
        /// <returns>
        /// The string builder with replaced HTML.
        /// </returns>
        StringBuilder GetReplacedHtml(StringBuilder stringBuilder, RenderPageViewModel model);
    }
}