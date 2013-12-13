namespace BetterCms.Module.Pages.Command.Page.AssignMainCulturePage
{
    public class AssignMainCulturePageCommandRequest
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the main culture page id.
        /// </summary>
        /// <value>
        /// The main culture page id.
        /// </value>
        public System.Guid MainCulturePageId { get; set; }

        /// <summary>
        /// Gets or sets the culture id.
        /// </summary>
        /// <value>
        /// The culture id.
        /// </value>
        public System.Guid CultureId { get; set; }
    }
}