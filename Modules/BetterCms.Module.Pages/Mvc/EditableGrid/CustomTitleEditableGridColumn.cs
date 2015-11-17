using System.Collections.Generic;

using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.Mvc.EditableGrid
{
    public class CustomTitleEditableGridColumn : EditableGridColumn
    {
        public CustomTitleEditableGridColumn(string headerTitle, string valueBind, 
            IDictionary<string, string> customBindings) : base(headerTitle, null, valueBind)
        {
            CellView = PagesConstants.CustomTitleCellTemplate;
            CustomBindings = customBindings;
        }

        public IDictionary<string, string> CustomBindings { get; set; }

    }
}