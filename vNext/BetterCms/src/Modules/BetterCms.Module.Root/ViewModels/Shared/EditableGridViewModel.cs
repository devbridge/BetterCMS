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