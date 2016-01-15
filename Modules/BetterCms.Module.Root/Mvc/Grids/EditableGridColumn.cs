// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableGridColumn.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridColumn
    {
        public virtual string HeaderTitle { get; set; }
        
        public virtual string SortColumn { get; set; }
        
        public virtual string ValueBind { get; set; }
        
        public virtual string HeaderAttributes { get; set; }
        
        public virtual string Attributes { get; set; }
        
        public virtual string HeaderView { get; set; }
        
        public virtual string CellView { get; set; }
        
        public virtual bool AutoFocus { get; set; }
        
        public virtual bool CanBeEdited { get; set; }
        
        public virtual bool IsRendered { get; set; }

        public virtual string HiddenFieldName { get; set; }

        public virtual string CustomBinding { get; set; }

        public virtual string FocusIdentifier { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableGridColumn" /> class.
        /// </summary>
        /// <param name="headerTitle">The header title.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="valueBind">The value to bind.</param>
        public EditableGridColumn(string headerTitle = null, string sortColumn = null, string valueBind = null)
        {
            HeaderTitle = headerTitle;
            SortColumn = sortColumn;
            ValueBind = valueBind ?? "''";
            HeaderView = RootModuleConstants.EditableGridHeaderTemplate;
            CellView = RootModuleConstants.EditableGridCellTemplate;
            CanBeEdited = true;
            IsRendered = true;
            FocusIdentifier = string.Empty;
        }
    }
}