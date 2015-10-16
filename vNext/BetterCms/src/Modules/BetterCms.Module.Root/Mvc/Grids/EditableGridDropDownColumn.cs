namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridDropDownColumn : EditableGridColumn
    {
        public virtual string OptionsBind { get; set; }
        
        public virtual string OptionsText { get; set; }
        
        public virtual string OptionsValue { get; set; }

        public virtual string ValueTextBind { get; set; }

        public EditableGridDropDownColumn(string headerTitle = null, string sortColumn = null, string valueBind = null)
            : base(headerTitle, sortColumn, valueBind)
        {
            CellView = RootModuleConstants.EditableGridDropDownCellTemplate;
            OptionsValue = "id";
            OptionsText = "name";
        }
    }
}