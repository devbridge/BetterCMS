using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Mvc.Grids.TableRenderers;

using MvcContrib.UI.Grid;

namespace BetterCms.Module.MediaManager.Mvc.List.ListRenderers
{
    /// <summary>
    /// Helper class to render a media items list.
    /// </summary>
    /// <typeparam name="T">Table item type</typeparam>
    public class MediaListRenderer<T> : EditableHtmlTableGridRenderer<T> where T : class
    {
        /// <summary>
        /// The default CSS class.
        /// </summary>
        private const string DefaultCssClass = "bcms-list-style";

        /// <summary>
        /// The default header CSS class.
        /// </summary>
        private const string DefaultHeaderCssClass = "bcms-media-sorting-block";

        /// <summary>
        /// The default header cell CSS class.
        /// </summary>
        private const string DefaultHeaderCellCssClass = "bcms-media-col-{0}";

        /// <summary>
        /// Renders the grid start.
        /// </summary>
        protected override void RenderGridStart()
        {
            if (!GridModel.Attributes.ContainsKey("class"))
            {
                GridModel.Attributes["class"] = DefaultCssClass;
            }

            string attrs = BuildHtmlAttributes(GridModel.Attributes);

            if (attrs.Length > 0)
            {
                attrs = " " + attrs;
            }

            RenderText(string.Format("<div{0}>", attrs));
        }

        /// <summary>
        /// Renders the grid end.
        /// </summary>
        /// <param name="isEmpty">if set to <c>true</c> [is empty].</param>
        protected override void RenderGridEnd(bool isEmpty)
        {
            RenderText("</div>");
        }

        /// <summary>
        /// Renders the head start.
        /// </summary>
        protected override void RenderHeadStart()
        {
            var headerAttributes = GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(null, false));

            if (!headerAttributes.ContainsKey("class"))
            {
                headerAttributes["class"] = DefaultHeaderCssClass;
            }

            string attributes = BuildHtmlAttributes(headerAttributes);

            if (attributes.Length > 0)
            {
                attributes = " " + attributes;
            }

            RenderText(string.Format("<div{0}>", attributes));
        }

        /// <summary>
        /// Renders the head end.
        /// </summary>
        protected override void RenderHeadEnd()
        {
            RenderText("</div>");
        }

        /// <summary>
        /// Renders the header cell start.
        /// </summary>
        /// <param name="column">The column.</param>
        protected override void RenderHeaderCellStart(GridColumn<T> column)
        {
            if (!column.HeaderAttributes.ContainsKey("class"))
            {
                column.HeaderAttributes["class"] = string.Format(DefaultHeaderCellCssClass, column.Position != null ? column.Position.Value + 1 : 1);
            }

            var attrs = BuildHtmlAttributes(column.HeaderAttributes);

            if (attrs.Length > 0)
            {
                attrs = " " + attrs;
            }

            RenderText(string.Format("<div{0}>", attrs));
        }

        /// <summary>
        /// Renders the header cell end.
        /// </summary>
        protected override void RenderHeaderCellEnd()
        {
            RenderText("</div>");
        }

        /// <summary>
        /// Renders the header cell start.
        /// </summary>
        protected void RenderHeaderCellStart()
        {
            RenderText("<div>");
        }

        /// <summary>
        /// Renders the row start.
        /// </summary>
        /// <param name="rowData">The row data.</param>
        protected override void RenderRowStart(GridRowViewData<T> rowData)
        {
            RenderText(string.Empty);
        }

        /// <summary>
        /// Renders the row end.
        /// </summary>
        protected override void RenderRowEnd()
        {
            RenderText(string.Empty);
        }

        /// <summary>
        /// Renders the start cell.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="rowData">The row data.</param>
        protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            RenderText(string.Empty);
        }

        /// <summary>
        /// Renders the cell value.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="rowData">The row data.</param>
        protected override void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            RenderText(string.Empty);
        }

        /// <summary>
        /// Renders the end cell.
        /// </summary>
        protected override void RenderEndCell()
        {
            RenderText(string.Empty);
        }

        /// <summary>
        /// Renders the empty.
        /// </summary>
        protected override void RenderEmpty()
        {
            RenderHeadStart();
            RenderHeaderCellStart();
            RenderHeaderCellEnd();
            RenderHeadEnd();
            RenderBodyStart();
            RenderEmptyRow(false);
            RenderBodyEnd();
        }

        /// <summary>
        /// Renders the body start.
        /// </summary>
        protected override void RenderBodyStart()
        {
        }

        /// <summary>
        /// Renders the body end.
        /// </summary>
        protected override void RenderBodyEnd()
        {
            if (!IsDataSourceEmpty())
            {
                RenderEmptyRow(true);
            }
        }

        private void RenderEmptyRow(bool isHidden)
        {
            var hidden = isHidden ? " style=\"display: none;\"" : string.Empty;
            RenderText(string.Format("<div class=\"bcms-list-empty-row\"{0}><span>{1}</span></div>", hidden, GridModel.EmptyText));
        }

        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns>String with attributes.</returns>
        private static new string BuildHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            return string.Join(" ", attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray());
        }
    }
}