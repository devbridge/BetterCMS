namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridMultilineColumn : EditableGridColumn
    {
        public EditableGridMultilineColumn(string headerTitle = null, string sortColumn = null, string valueBind = null)
            : base(headerTitle, sortColumn, valueBind)
        {
            CellView = RootModuleConstants.EditableGridMultilineTextCellTemplate;
        }
    }
}