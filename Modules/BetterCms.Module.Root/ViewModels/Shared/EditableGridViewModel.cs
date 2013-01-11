using System.Collections.Generic;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Root.ViewModels.Shared
{
    public class EditableGridViewModel
    {
        public virtual string AddNewTitle { get; set; }

        public virtual string TopBlockView { get; set; }

        public virtual IList<EditableGridColumn> Columns { get; set; }

        public EditableGridViewModel()
        {
            TopBlockView = RootModuleConstants.EditableGridTopBlockTemplate;
            AddNewTitle = RootGlobalization.Button_AddNew;
            Columns = new List<EditableGridColumn>();
        }
    }
}