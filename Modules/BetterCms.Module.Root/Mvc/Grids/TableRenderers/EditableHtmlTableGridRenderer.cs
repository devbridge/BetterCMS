using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Content.Resources;

using MvcContrib.Sorting;
using MvcContrib.UI.Grid;

namespace BetterCms.Module.Root.Mvc.Grids.TableRenderers
{
    /// <summary>
    /// Helper class for rendering HTML tables for editing data with sorting
    /// </summary>
    /// <typeparam name="T">Table item type</typeparam>
    public class EditableHtmlTableGridRenderer<T> : HtmlTableGridRenderer<T> where T : class
    {
        /// <summary>
        /// Renders head section.
        /// </summary>
        /// <returns>Rendered header</returns>
        protected override bool RenderHeader()
        {
            if (!ShouldRenderHeader())
            {
                return false;
            }

            RenderHeadStart();

            foreach (var column in VisibleColumns())
            {
               RenderHeaderCellStart(column);
               RenderHeaderText(column);
               RenderHeaderCellEnd();
            }

            RenderHeadEnd();

            return !IsDataSourceEmpty();
        }

        /// <summary>
        /// Renders the header text.
        /// </summary>
        /// <param name="column">The column.</param>
        protected override void RenderHeaderText(GridColumn<T> column)
        {
            if (IsSortingEnabled && column.Sortable)
            {
                bool isSortedByThisColumn = GridModel.SortOptions.Column == GenerateSortColumnName(column);

                string sortClass;
                if (isSortedByThisColumn)
                {
                    sortClass = GridModel.SortOptions.Direction == SortDirection.Ascending
                        ? "bcms-sort-arrow-top"
                        : "bcms-sort-arrow-bottom";
                }
                else
                {
                    sortClass = null;
                }

                var displayName = column.DisplayName;

                string sortColumnName = GenerateSortColumnName(column);
                var sortOptions = new GridSortOptions
                {
                    Column = sortColumnName
                };

                sortOptions.Direction = (GridModel.SortOptions.Direction == SortDirection.Ascending && isSortedByThisColumn)
                                            ? SortDirection.Descending
                                            : SortDirection.Ascending;

                var link = string.Format("<a class=\"{0} bcms-sort-arrow\" data-column=\"{1}\" data-direction=\"{2}\">{3}</a>",
                    sortClass,
                    sortOptions.Column,
                    sortOptions.Direction,
                    displayName);

                RenderText(link);
            }
            else
            {
                RenderText(column.DisplayName);
            }
        }

        protected override void RenderEmpty()
        {
            RenderBodyStart();
            RenderEmptyRow(false);
            RenderBodyEnd();
        }

        protected override bool ShouldRenderHeader()
        {
            return true;
        }

        protected override void RenderBodyStart()
        {
            this.RenderText("<div class=\"bcms-tables-tbody\">");
        }

        protected override void RenderBodyEnd()
        {
            if (!IsDataSourceEmpty())
            {
                RenderEmptyRow(true);
            }
//            base.RenderBodyEnd();
            this.RenderText("</div>");
        }

        protected override void RenderHeadStart()
        {
            string str = this.BuildHtmlAttributes(this.GridModel.Sections.HeaderRow.Attributes(new GridRowViewData<T>(default(T), false)));
            if (str.Length > 0)
                str = " " + str;
            this.RenderText(string.Format("<div class=\"bcms-thead\"><div{0}>", (object)str));
        }

        protected override void RenderHeadEnd()
        {
            this.RenderText("</div></div>");
        }

        protected override void RenderGridStart()
        {
            if (!this.GridModel.Attributes.ContainsKey("class"))
                this.GridModel.Attributes["class"] = (object)"grid";
            string str = this.BuildHtmlAttributes(this.GridModel.Attributes);
            if (str.Length > 0)
                str = " " + str;
            this.RenderText(string.Format("<div{0}>", (object)str));
        }

        protected override void RenderGridEnd(bool isEmpty)
        {
            this.RenderText("</div>");
        }

        protected override void RenderRowStart(GridRowViewData<T> rowData)
        {
            var attributes = GridModel.Sections.Row.Attributes(rowData);
            if (!attributes.ContainsKey("class"))
            {
                attributes["class"] = rowData.IsAlternate ? "bcms-tables-tr gridrow_alternate" : "bcms-tables-tr gridrow";
            }
            else
            {
                attributes["class"] = string.Format("{0} {1}", attributes["class"], "bcms-tables-tr");
            }
            var str = BuildHtmlAttributes(attributes);
            if (str.Length > 0)
            {
                str = " " + str;
            }
            RenderText(string.Format("<div{0}>", str));
        }

        protected override void RenderRowEnd()
        {
            this.RenderText("</div>");
        }

        protected override void RenderEndCell()
        {
            this.RenderText("</div>");
        }

        protected override void RenderStartCell(GridColumn<T> column, GridRowViewData<T> rowData)
        {
            var attributes = column.Attributes(rowData);
            if (!attributes.ContainsKey("class"))
                attributes["class"] = (object)"bcms-tables-td";
            string str = this.BuildHtmlAttributes(attributes);
            if (str.Length > 0)
                str = " " + str;
            this.RenderText(string.Format("<div{0}>", (object)str));
        }

        protected override void RenderHeaderCellStart(GridColumn<T> column)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>(column.HeaderAttributes);
            if (this.IsSortingEnabled && column.Sortable && this.GridModel.SortOptions.Column == this.GenerateSortColumnName(column))
            {
                string str = this.GridModel.SortOptions.Direction == SortDirection.Ascending ? "sort_asc" : "sort_desc";
                if (dictionary.ContainsKey("class") && dictionary["class"] != null)
                    str = string.Join(" ", dictionary["class"].ToString(), str);
                dictionary["class"] = (object)str;
            }
            string str1 = this.BuildHtmlAttributes((IDictionary<string, object>)dictionary);
            if (str1.Length > 0)
                str1 = " " + str1;
            this.RenderText(string.Format("<div{0}>", (object)str1));
        }

        protected override void RenderHeaderCellEnd()
        {
            this.RenderText("</div>");
        }

        private void RenderEmptyRow(bool isHidden)
        {
            var hidden = isHidden ? " style=\"display: none;\"" : string.Empty;
//            RenderText(string.Format("<tr class=\"bcms-grid-empty-row\"{0}><td colspan=\"{1}\"><span class=\"bcms-table-no-data\">{2}</span></td></tr>", hidden, VisibleColumns().Count(), RootGlobalization.Grid_NoDataAvailable_Message));
            RenderText(string.Format("<div class=\"bcms-grid-empty-row\"{0}><div colspan=\"{1}\"><span class=\"bcms-table-no-data\">{2}</span></div></div>", hidden, VisibleColumns().Count(), RootGlobalization.Grid_NoDataAvailable_Message));
        }
    }
}