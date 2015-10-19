using System.Text;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// HTML form helper.
    /// </summary>
    public static class FormHelper
    {
        /// <summary>
        /// Renders hidden submit button.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <returns>Rendered hidden submit.</returns>
        public static IHtmlContent HiddenSubmit(this IHtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<input type=\"submit\" style=\"position: absolute; left: -999em; top: -999em;\" />");
            return new HtmlString(sb.ToString());
        }
    }
}