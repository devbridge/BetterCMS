using System.Collections.Generic;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Root.ViewModels.Shared
{
    public class EditableGridViewModel
    {
        public virtual string AddNewTitle { get; set; }
        
        public virtual string TopBlockTitle { get; set; }

        public virtual string TopBlockView { get; set; }

        public virtual string TopBlockClass { get; set; }

        public virtual bool CanAddNewItems { get; set; }
        
        public virtual bool CanEditItems { get; set; }
        
        public virtual bool CanDeleteItems { get; set; }

        public virtual IList<EditableGridColumn> Columns { get; set; }

        public EditableGridViewModel()
        {
            TopBlockView = RootModuleConstants.EditableGridTopBlockTemplate;
            TopBlockClass = RootModuleConstants.EditableGridTopBlockClassName;
            AddNewTitle = RootGlobalization.Button_AddNew;
            Columns = new List<EditableGridColumn>();
            
            CanAddNewItems = true;
            CanEditItems = true;
            CanDeleteItems = true;
        }

        public override string ToString()
        {
            return string.Format("Columns count: {0}", Columns.Count);
        }
    }
}