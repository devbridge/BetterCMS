using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc.Grids.TableRenderers;

using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

namespace BetterCms.Module.MediaManager.Mvc.Grid.TableRenderers
{
    /// <summary>
    /// Helper class to render a media items table.
    /// </summary>
    /// <typeparam name="T">Table item type</typeparam>
    public class MediaGridRenderer<T> : EditableHtmlTableGridRenderer<T> where T : class
    {
        private const string DefaultCssClass = "bcms-list-style";
        private const string DefaultHeaderCssClass = "bcms-media-sorting-block";
        private const string DefaultHeaderCellCssClass = "bcms-media-col-{0}";

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

        protected override void RenderGridEnd(bool isEmpty)
        {
            RenderText("</div>");
        }

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

        protected override void RenderHeadEnd()
        {
            RenderText("</div>");
        }

        protected override void RenderHeaderCellStart(GridColumn<T> column)
        {
            if (!column.HeaderAttributes.ContainsKey("class"))
            {
                column.HeaderAttributes["class"] = string.Format(DefaultHeaderCellCssClass, column.Position != null ? column.Position.Value + 1 : 1);
            }
            string attrs = BuildHtmlAttributes(column.HeaderAttributes);

            if (attrs.Length > 0)
            {
                attrs = " " + attrs;
            }

            RenderText(string.Format("<div{0}>", attrs));
        }

        protected override void RenderHeaderCellEnd()
        {
            RenderText("</div>");
        }

        protected void RenderHeaderCellStart()
        {
            RenderText("<div>");
        }

        protected override void RenderRowStart(GridRowViewData<T> rowData)
        {
            var attributes = GridModel.Sections.Row.Attributes(rowData);

            string attributeString = BuildHtmlAttributes(attributes);

            if (attributeString.Length > 0)
            {
                attributeString = " " + attributeString;
            }

            RenderText(string.Format("<div{0}>", attributeString));
        }

        protected override void RenderRowEnd()
        {
            RenderText("</div>");
        }

        protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
        {            
            string attrs = BuildHtmlAttributes(column.Attributes(rowData));
            if (attrs.Length > 0)
            {
                attrs = " " + attrs;
            }

            RenderText(string.Format("<div{0}>", attrs));
        }

        protected override void RenderCellValue(GridColumn<T> column, GridRowViewData<T> rowData)
        {            
            base.RenderCellValue(column, rowData);
        }

        protected override void RenderEndCell()
        {
            RenderText("</div>");
        }

        protected override void RenderEmpty()
        {
            RenderHeadStart();
            RenderHeaderCellStart();
            RenderHeaderCellEnd();
            RenderHeadEnd();
            RenderBodyStart();
            RenderText("<div><span>" + GridModel.EmptyText + "</span></div>");
            RenderBodyEnd();
        }

        protected override void RenderBodyStart()
        {            
        }

        protected override void RenderBodyEnd()
        {         
        }

        /// <summary>
        /// Converts the specified attributes dictionary of key-value pairs into a string of HTML attributes. 
        /// </summary>
        /// <returns></returns>
        private static string BuildHtmlAttributes(IDictionary<string, object> attributes)
        {
            if (attributes == null || attributes.Count == 0)
            {
                return string.Empty;
            }

            const string attributeFormat = "{0}=\"{1}\"";

            return string.Join(" ", attributes.Select(pair => string.Format(attributeFormat, pair.Key, pair.Value)).ToArray()
            );
        }
    }
}