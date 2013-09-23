namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridHiddenField : EditableGridColumn
    {
        public EditableGridHiddenField(string valueBind, string hiddenFieldName)
            : base(null, null, valueBind)
        {
            HiddenFieldName = hiddenFieldName;
            IsRendered = false;
        }
    }
}