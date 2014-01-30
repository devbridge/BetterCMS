using System.Web;

using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Search.Helpers
{
    public static class RenderWidgetViewModelExtensions
    {
        public static string GetSearchQueryParameter(this RenderWidgetViewModel model, HttpRequestBase request, string requestQuery = null)
        {
            var queryParameterName = model.GetOptionValue<string>(SearchModuleConstants.WidgetOptionNames.QueryParameterName);
            if (!string.IsNullOrEmpty(queryParameterName))
            {
                requestQuery = System.Web.Helpers.Validation.Unvalidated(request, queryParameterName);
            }

            return requestQuery;
        }
    }
}