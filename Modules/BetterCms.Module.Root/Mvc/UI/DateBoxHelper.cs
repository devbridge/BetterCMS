using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetterCms.Module.Root.Mvc.UI
{
    public static class DateBoxHelper
    {
        private const string DefaultDateFormat = "MM'/'dd'/'yyyy";

        public static MvcHtmlString DateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DateTime? value, IDictionary<string, object> htmlAttributes = null)
        {
            if (value.HasValue)
            {
                string date = value.Value.ToString(DefaultDateFormat);
                htmlAttributes = htmlAttributes ?? new Dictionary<string, object>(1);
                htmlAttributes["Value"] = date;
            }
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }
    }
}