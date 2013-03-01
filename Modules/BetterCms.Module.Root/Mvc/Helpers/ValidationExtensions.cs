using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

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
    }
}