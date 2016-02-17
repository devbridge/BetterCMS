// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListModelBindingExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// List model binding extensions. 
    /// </summary>
    /// <remarks>Helper for non-sequence list</remarks>
    public static class ListModelBindingExtensions
    {
        /// <summary>
        /// The strip indexer regex
        /// </summary>
        static Regex stripIndexerRegex = new Regex(@"\[(?<index>\d+)\]", RegexOptions.Compiled);

        /// <summary>
        /// Gets the name of the indexer field.
        /// </summary>
        /// <param name="templateInfo">The template info.</param>
        /// <returns></returns>
        public static string GetIndexerFieldName(this TemplateInfo templateInfo)
        {
            string fieldName = templateInfo.GetFullHtmlFieldName("Index");
            fieldName = stripIndexerRegex.Replace(fieldName, string.Empty);
            if (fieldName.StartsWith(".", StringComparison.Ordinal))
            {
                fieldName = fieldName.Substring(1);
            }
            return fieldName;
        }

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <param name="templateInfo">The template info.</param>
        /// <returns></returns>
        public static int GetIndex(this TemplateInfo templateInfo)
        {
            string fieldName = templateInfo.GetFullHtmlFieldName("Index");
            var match = stripIndexerRegex.Match(fieldName);
            if (match.Success)
            {
                return int.Parse(match.Groups["index"].Value);
            }
            return 0;
        }

        /// <summary>
        /// Hiddens the indexer input for model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenIndexerInputForModel<TModel>(this HtmlHelper<TModel> html, string className = null)
        {
            string name = html.ViewData.TemplateInfo.GetIndexerFieldName();
            object value = html.ViewData.TemplateInfo.GetIndex();

            return CreateIndexerInput(name, value, className);
        }

        /// <summary>
        /// Hiddens the indexer input for model.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenIndexerInputForModel<TModel, TValue>(this HtmlHelper<TModel> html, string className, Expression<Func<TModel, TValue>> expression)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string name = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText) + ".Index";
            name = stripIndexerRegex.Replace(name, string.Empty);
            if (name.StartsWith(".", StringComparison.Ordinal))
            {
                name = name.Substring(1);
            }

            object value = html.ViewData.TemplateInfo.GetIndex();

            return CreateIndexerInput(name, value, className);
        }

        /// <summary>
        /// Creates the indexer input.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        private static MvcHtmlString CreateIndexerInput(string name, object value, string className)
        {
            string markup;
            if (string.IsNullOrEmpty(className))
            {
                markup = string.Format(@"<input type=""hidden"" name=""{0}""  value=""{1}"" />", name, value);
            }
            else
            {
                markup = string.Format(@"<input type=""hidden"" name=""{0}"" class=""{2}"" value=""{1}"" />", name, value, className);
            }
            return MvcHtmlString.Create(markup);
        }

        /// <summary>
        /// Hidden fake input for templates
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public static MvcHtmlString HiddenInputForFake<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, string className = null)
        {
            string expressionText = ExpressionHelper.GetExpressionText(expression);
            string labelId = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(expressionText);
            string name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expressionText);

            MvcHtmlString result;
            if (string.IsNullOrEmpty(className))
            {
                result = htmlHelper.Hidden(name, null, new { id = labelId });
            }
            else
            {
                result = htmlHelper.Hidden(name, null, new { id = labelId, @class = className });
            }

            return result;
        }
    }
}