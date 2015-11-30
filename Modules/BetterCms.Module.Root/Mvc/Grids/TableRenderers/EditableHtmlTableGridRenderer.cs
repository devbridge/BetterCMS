// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableHtmlTableGridRenderer.cs" company="Devbridge Group LLC">
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

                var link = string.Format("<div class=\"{0} bcms-sort-arrow\" data-column=\"{1}\" data-direction=\"{2}\">{3}</a>",
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
            RenderText(string.Format("<tr class=\"bcms-grid-empty-row\"{0}><td colspan=\"{1}\"><div class=\"bcms-table-no-data\">{2}</div></td></tr>", hidden, VisibleColumns().Count(), RootGlobalization.Grid_NoDataAvailable_Message));
        }
    }
}