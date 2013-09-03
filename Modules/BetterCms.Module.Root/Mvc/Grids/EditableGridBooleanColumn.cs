namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridBooleanColumn : EditableGridColumn
    {
        public EditableGridBooleanColumn(string headerTitle = null, string sortColumn = null, string valueBind = null)
            : base(headerTitle, sortColumn, valueBind)
        {
            CellView = RootModuleConstants.EditableGridBooleanCellTemplate;
        }
    }
}