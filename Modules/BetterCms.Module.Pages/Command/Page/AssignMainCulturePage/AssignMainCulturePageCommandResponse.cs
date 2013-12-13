namespace BetterCms.Module.Pages.Command.Page.AssignMainCulturePage
{
    public class AssignMainCulturePageCommandResponse
    {
        /// <summary>
        /// Gets or sets the main culture page id.
        /// </summary>
        /// <value>
        /// The main culture page id.
        /// </value>
        public System.Guid MainCulturePageId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public string PageUrl { get; set; }
    }
}