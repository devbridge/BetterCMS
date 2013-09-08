namespace BetterCms.Module.Root.ViewModels.Security
{
    public interface IAccessSecuredViewModel
    {        
        /// <summary>
        /// Gets or sets a value indicating whether dialog should be opened in the read only mode.
        /// </summary>
        /// <value>
        /// <c>true</c> if dialog should be opened in the read only mode; otherwise, <c>false</c>.
        /// </value>
        bool IsReadOnly { get; set; }
    }
}
