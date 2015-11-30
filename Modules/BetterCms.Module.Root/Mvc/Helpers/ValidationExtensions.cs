// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationExtensions.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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