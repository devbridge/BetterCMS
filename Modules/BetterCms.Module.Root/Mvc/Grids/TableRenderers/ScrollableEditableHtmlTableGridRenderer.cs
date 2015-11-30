// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrollableEditableHtmlTableGridRenderer.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Root.Mvc.Grids.TableRenderers
{
    /// <summary>
    /// Helper class for rendering HTML tables for editing data with sorting
    /// </summary>
    /// <typeparam name="T">Table item type</typeparam>
    public class ScrollableEditableHtmlTableGridRenderer<T> : EditableHtmlTableGridRenderer<T> where T : class
    {
        public string InternalTableCssClass { get; set; }

        protected override void RenderBodyStart()
        {
            RenderText("<tbody>");
            RenderText("<tr>");
            RenderText(string.Format("<td colspan=\"{0}\" class=\"{1}\" style=\"border:0; padding: 0;\">", GridModel.Columns.Count, InternalTableCssClass));
            RenderText("<div class=\"bcms-scroll-tbody\">");
            RenderGridStart();
            base.RenderBodyStart();
        }

        protected override void RenderBodyEnd()
        {
            base.RenderBodyEnd();
            RenderGridEnd(false);
            RenderText("</div>");
            RenderText("</td>");
            RenderText("</tr>");
            RenderText("</tbody>");
        }
    }
}