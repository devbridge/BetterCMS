using System.Text;
using System.Web;
using System.Web.Mvc;

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
        public static IHtmlString HiddenSubmit(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<input type=\"submit\" style=\"position: absolute; left: -999em; top: -999em;\" />");
            return new MvcHtmlString(sb.ToString());
        }
    }
}