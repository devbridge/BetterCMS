namespace BetterCms.Module.Root.Mvc.Grids
{
    public class EditableGridColumn
    {
        public virtual string HeaderTitle { get; set; }
        
        public virtual string SortColumn { get; set; }
        
        public virtual string ValueBind { get; set; }
        
        public virtual string HeaderAttributes { get; set; }
        
        public virtual string Attributes { get; set; }
        
        public virtual string HeaderView { get; set; }
        
        public virtual string CellView { get; set; }
        
        public virtual bool AutoFocus { get; set; }
        
        public virtual bool CanBeEdited { get; set; }
        
        public virtual bool IsRendered { get; set; }

        public virtual string HiddenFieldName { get; set; }

        public virtual string CustomBinding { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditableGridColumn" /> class.
        /// </summary>
        /// <param name="headerTitle">The header title.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="valueBind">The value to bind.</param>
        public EditableGridColumn(string headerTitle = null, string sortColumn = null, string valueBind = null)
        {
            HeaderTitle = headerTitle;
            SortColumn = sortColumn;
            ValueBind = valueBind ?? "''";
            HeaderView = RootModuleConstants.EditableGridHeaderTemplate;
            CellView = RootModuleConstants.EditableGridCellTemplate;
            CanBeEdited = true;
            IsRendered = true;
        }
    }
}