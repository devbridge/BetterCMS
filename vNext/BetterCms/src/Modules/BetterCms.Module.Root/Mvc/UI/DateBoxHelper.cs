using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetterCms.Module.Root.Mvc.UI
{
    public static class DateBoxHelper
    {
        public static MvcHtmlString DateTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DateTime? value, IDictionary<string, object> htmlAttributes = null)
        {
            if (value.HasValue)
            {
                string date = value.Value.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                htmlAttributes = htmlAttributes ?? new Dictionary<string, object>(1);
                htmlAttributes["Value"] = date;
            }
            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }
    }
}