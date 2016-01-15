// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableGridViewModel.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Root.ViewModels.Shared
{
    public class EditableGridViewModel
    {
        public virtual string AddNewTitle { get; set; }
        
        public virtual string TopBlockTitle { get; set; }

        public virtual string SaveButtonTitle { get; set; }

        public virtual string TopBlockView { get; set; }

        public virtual string TopBlockAddItemView { get; set; }
        
        public virtual string PagingView { get; set; }

        public virtual string TopBlockClass { get; set; }

        public virtual bool CanAddNewItems { get; set; }
        
        public virtual bool ShowSearch { get; set; }
        
        public virtual bool CanEditItems { get; set; }
        
        public virtual bool CanDeleteItems { get; set; }
        
        public virtual bool AddPaging { get; set; }

        public virtual bool AddHiddenFields { get; set; }
        
        public virtual bool ShowMessages { get; set; }

        public virtual List<EditableGridColumn> Columns { get; set; }

        public EditableGridViewModel()
        {
            TopBlockView = RootModuleConstants.EditableGridTopBlockTemplate;
            TopBlockAddItemView = RootModuleConstants.EditableGridTopBlockAddItemTemplate;
            PagingView = RootModuleConstants.EditableGridPagingTemplate;
            TopBlockClass = RootModuleConstants.EditableGridTopBlockClassName;
            AddNewTitle = RootGlobalization.Button_AddNew;
            SaveButtonTitle = RootGlobalization.Button_Save;
            Columns = new List<EditableGridColumn>();

            ShowSearch = true;
            CanAddNewItems = true;
            CanEditItems = true;
            CanDeleteItems = true;
            AddPaging = true;
            AddHiddenFields = false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Columns count: {0}", Columns.Count);
        }
    }
}