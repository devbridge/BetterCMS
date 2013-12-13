namespace BetterCms.Module.Pages.Command.Page.GetPageForCloningWithCulture
{
    public class GetPageForCloningWithCultureCommandRequest
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the culture id.
        /// </summary>
        /// <value>
        /// The culture id.
        /// </value>
        public System.Guid CultureId { get; set; }
    }
}