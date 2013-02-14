using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using BetterCms.Module.Pages.Accessors;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.ViewModels.Content;

using Common.Logging;

namespace BetterCms.Module.Pages.Helpers
{
    public static class RenderHelper
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static IHtmlString RenderWidgetPreview(this HtmlHelper html, string widgetTitle, string widgetUrl, ContentViewModel model)
        {
            try
            {
                var viewData = new ViewDataDictionary();

                if (model != null && model.ContentOptions != null && model.ContentOptions.Count > 0)
                {
                    foreach (var option in model.ContentOptions)
                    {
                        if (option.OptionDefaultValue != null)
                        {
                            viewData[option.OptionKey] = option.OptionDefaultValue;
                        }
                    }
                }

                return html.Partial(widgetUrl, model, viewData);
            }
            catch (Exception ex)
            {
                string message = string.Format(PagesGlobalization.WidgetPreview_Failed, widgetTitle, ex.Message.TrimEnd('.'));
                Log.Error(message, ex);

                StringBuilder sb = new StringBuilder();
                sb.Append("<b>");
                sb.Append(widgetTitle);
                sb.AppendLine("</b>");
                sb.AppendLine("<br/>");
                sb.Append("<small>");
                sb.Append(message);
                sb.AppendLine("</small>");

                return new MvcHtmlString(sb.ToString());
            }
        }
    }
}