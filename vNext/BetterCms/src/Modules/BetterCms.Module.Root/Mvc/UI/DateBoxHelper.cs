using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using Microsoft.AspNet.Html.Abstractions;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Module.Root.Mvc.UI
{
    public static class DateBoxHelper
    {
        public static IHtmlContent DateTextBoxFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, DateTime? value, IDictionary<string, object> htmlAttributes = null)
        {
            if (value.HasValue)
            {
                string date = value.Value.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                htmlAttributes = htmlAttributes ?? new Dictionary<string, object>(1);
                htmlAttributes["Value"] = date;
            }
            //TODO what format should be used?
            return htmlHelper.TextBoxFor(expression, "" ,htmlAttributes);
        }
    }
}