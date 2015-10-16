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

        protected override void RenderBodyEnd()
        {
            if (!IsDataSourceEmpty())
            {
                RenderEmptyRow(true);
            }
            base.RenderBodyEnd();
        }

        private void RenderEmptyRow(bool isHidden)
        {
            var hidden = isHidden ? " style=\"display: none;\"" : string.Empty;
            RenderText(string.Format("<tr class=\"bcms-grid-empty-row\"{0}><td colspan=\"{1}\"><span class=\"bcms-table-no-data\">{2}</span></td></tr>", hidden, VisibleColumns().Count(), RootGlobalization.Grid_NoDataAvailable_Message));
        }
    }
}