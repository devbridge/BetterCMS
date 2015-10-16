using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// Merges given attributes with view model property's validation attributes.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>
        /// Attributes, merged with view model property's validation attributes
        /// </returns>
        public static Dictionary<string, object> MergeValidationAttributes<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, Dictionary<string, object> attributes)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var validationAttributes = htmlHelper.GetUnobtrusiveValidationAttributes(Guid.NewGuid().ToString(), metadata);

            foreach (var pair in validationAttributes)
            {
                if (!attributes.ContainsKey(pair.Key))
                {
                    attributes.Add(pair.Key, pair.Value);
                }
            }

            return attributes;
        }

        /// <summary>
        /// MVC's ValidationMessageFor extender, replaces MVC class with bcms-class.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// HTML string with replaces CSS class name
        /// </returns>
        public static MvcHtmlString BcmsValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage = null, object htmlAttributes = null)
        {
            var result = htmlHelper.ValidationMessageFor(expression, validationMessage, htmlAttributes).ToHtmlString();
            result = result.Replace("field-validation-valid", "bcms-field-validation-valid");

            return new MvcHtmlString(result);
        }
    }
}