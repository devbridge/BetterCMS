using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Helpers;

using MvcContrib.UI.Grid;

namespace BetterCms.Module.Root.Mvc.Grids.Extensions
{
    public static class GridExtensions
    {
        public static IGridColumn<T> EmptyColumn<T>(this ColumnBuilder<T> builder) where T : class
        {
            return builder.For(f => "&nbsp;")
                .Named("&nbsp;")
                .Sortable(false)
                .Encode(false);
        }

        public static IGridColumn<T> EditButtonColumn<T>(this ColumnBuilder<T> builder, bool renderId = true) where T : class
        {
            return builder
                .For(f => string.Format("<a class=\"bcms-icn-edit bcms-grid-item-edit-button\"{0}>{1}</a>",
                        renderId && f is IEditableGridItem
                            ? string.Format("data-id=\"{0}\"", ((IEditableGridItem)f).Id)
                            : string.Empty,
                        RootGlobalization.Button_Edit))
                .Named("&nbsp;")
                .Sortable(false)
                .Encode(false)
                .HeaderAttributes(@class => "bcms-tables-nohover");
        }

        public static IGridColumn<T> HistoryButtonColumn<T>(this ColumnBuilder<T> builder, bool renderId = true) where T : class
        {
            return builder
                .For(f => string.Format("<a class=\"bcms-icn-history bcms-grid-item-history-button\"{0}>{1}</a>",
                        renderId && f is IEditableGridItem
                            ? string.Format("data-id=\"{0}\"", ((IEditableGridItem)f).Id)
                            : string.Empty,
                        RootGlobalization.Button_History))
                .Named("&nbsp;")
                .Sortable(false)
                .Encode(false)
                .HeaderAttributes(@style => "width: 80px;", @class => "bcms-tables-nohover");
        }

        public static IGridColumn<T> DeleteButtonColumn<T>(this ColumnBuilder<T> builder, bool renderId = true) where T : class
        {
            return builder
                .For(f => string.Format(
                            "<a class=\"bcms-icn-delete bcms-grid-item-delete-button\"{0}>{1}</a><div style=\"display:none\" class=\"bcms-grid-item-message\"></div>",
                            renderId && f is IEditableGridItem
                                ? string.Format("data-id=\"{0}\" data-version=\"{1}\"", ((IEditableGridItem)f).Id, ((IEditableGridItem)f).Version)
                                : string.Empty,
                            RootGlobalization.Button_Delete)
                    )
                .Named("&nbsp;")
                .Sortable(false)
                .Encode(false)
                .HeaderAttributes(@style => "width: 80px;", @class => "bcms-tables-nohover");
        }

        public static IGridColumn<T> InlineEditControlsColumn<T>(this ColumnBuilder<T> builder, string saveButtonTitle = null) where T : class
        {
            saveButtonTitle = saveButtonTitle ?? @RootGlobalization.Button_Save;

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<a class=\"bcms-icn-delete bcms-grid-item-delete-button\" data-id=\"{{0}}\" data-version=\"{{1}}\">{0}</a>", RootGlobalization.Button_Delete).AppendLine();
            stringBuilder.AppendFormat("<div style=\"display:none\" class=\"bcms-btn-small\">{0}</div>", saveButtonTitle).AppendLine();
            stringBuilder.AppendFormat("<a style=\"display:none\" class=\"bcms-btn-links-small\">{0}</a>", @RootGlobalization.Button_Cancel).AppendLine();
            stringBuilder.AppendFormat("<div style=\"display:none\" class=\"bcms-grid-item-message\"></div>");

            return builder
                .For(f => string.Format(stringBuilder.ToString(),
                            f is IEditableGridItem
                                ? ((IEditableGridItem)f).Id
                                : Guid.Empty,
                            f is IEditableGridItem
                                ? ((IEditableGridItem)f).Version
                                : 0)
                    )
                .Named("&nbsp;")
                .Sortable(false)
                .Encode(false)
                .HeaderAttributes(@style => "width: 145px;", @class => "bcms-tables-nohover");
        }

        public static HtmlString HiddenGridOptions(this HtmlHelper html, GridOptions.GridOptions gridOptions)
        {
            var column = html.Hidden("Column", gridOptions.Column, new {@id = "bcms-grid-sort-column"});
            var direction = html.Hidden("Direction", gridOptions.Direction, new { @id = "bcms-grid-sort-direction" });
            var pageNumber = html.Hidden("PageNumber", gridOptions.PageNumber, new { @id = "bcms-grid-page-number" });

            return new HtmlString(string.Concat(column.ToString(), direction.ToString(), pageNumber.ToString()));
        }

        /// <summary>
        /// Creates column with in-line editing options
        /// </summary>
        public static MvcHtmlString InlineEditColumn<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string textBoxClassName, string hiddenFieldClassName)
        {
            return CreateInlineEditColumn(htmlHelper, expression, textBoxClassName, hiddenFieldClassName, null);
        }

        /// <summary>
        /// Creates column with in-line editing options and fields name pattern for auto-generated names.
        /// </summary>
        public static MvcHtmlString InlineEditColumnWithNamePattern<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string textBoxClassName, string hiddenFieldClassName, string namePattern)
        {
            return CreateInlineEditColumn(htmlHelper, expression, textBoxClassName, hiddenFieldClassName, namePattern);
        }

        /// <summary>
        /// Creates non-editable column with text and hidden field with value.
        /// </summary>
        public static MvcHtmlString ColumnWithHiddenField<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string namePattern)
        {
            var model = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            string text = model != null ? Convert.ToString(model) : "&nbsp;";

            var inputAttributes = new Dictionary<string, object>
                                        {
                                            {"id", null},
                                            {"Name", namePattern},
                                            {"class", "bcms-editor-field-box"}
                                        };
            if (!string.IsNullOrWhiteSpace(namePattern))
            {
                inputAttributes.Add("data-name-pattern", namePattern);
            }

            // Hidden field
            var hiddenField = htmlHelper.HiddenFor(expression, inputAttributes);
            return new MvcHtmlString(string.Concat(text, hiddenField.ToString()));
        }

        /// <summary>
        /// Creates column with in-line editing options.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="textBoxClassName">Name of the text box class.</param>
        /// <param name="hiddenFieldClassName">The hidden field class namem.</param>
        /// <param name="namePattern">The auto-generated name pattern.</param>
        /// <returns>
        /// MVC HTML string with link, text box and hidden field
        /// </returns>
        private static MvcHtmlString CreateInlineEditColumn<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, string textBoxClassName, string hiddenFieldClassName, string namePattern)
        {
            var model = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).Model;
            string text = model != null ? Convert.ToString(model) : null;

            // Link
            TagBuilder link = new TagBuilder("a");
            link.SetInnerText(text);
            link.AddCssClass("bcms-tables-link bcms-grid-item-edit-button bcms-grid-item-info");
            link.AddCssClass(textBoxClassName);

            // Text box
            var textBoxAttributes = new Dictionary<string, object>
                                        {
                                            {"id", null},
                                            {"name", namePattern},
                                            {"style", "display:none; width:100%;"},
                                            {"class", string.Format("bcms-editor-field-box {0}", textBoxClassName)}
                                        };
            if (!string.IsNullOrWhiteSpace(namePattern))
            {
                textBoxAttributes.Add("data-name-pattern", namePattern);
            }

            // Merge validation attributes: add fake name, because 
            // attributes are not returned second time for same input
            textBoxAttributes = htmlHelper.MergeValidationAttributes(expression, textBoxAttributes);

            var textBox = htmlHelper.TextBoxFor(expression, textBoxAttributes);

            // Hidden field
            var hiddenField = htmlHelper.HiddenFor(expression, new { @id = (string)null, @Name = (string)null, @class = hiddenFieldClassName });

            // Validation box
            var validationBox = htmlHelper.BcmsValidationMessageFor(expression);

            // Div
            var div = new TagBuilder("div");
            div.InnerHtml = string.Concat(link.ToString(TagRenderMode.Normal), textBox.ToString(), validationBox.ToString(), hiddenField.ToString());
            div.AddCssClass("bcms-input-box");

            return new MvcHtmlString(div.ToString());
        }
    }
}