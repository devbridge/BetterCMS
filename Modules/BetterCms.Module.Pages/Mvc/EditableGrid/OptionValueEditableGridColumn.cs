using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Pages.Mvc.EditableGrid
{
    public class OptionValueEditableGridColumn : EditableGridColumn
    {
        public OptionValueEditableGridColumn(string headerTitle, string valueBind, string customValueBind)
            : base(headerTitle, null, valueBind)
        {
            CellView = RootModuleConstants.EditableOptionValueCellTemplate;
            CustomValueBind = customValueBind;
        }

        public string CustomValueBind { get; set; }
    }
}