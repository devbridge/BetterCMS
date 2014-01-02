namespace BetterCms.Module.Pages.Command.Page.GetPageForCloningWithLanguage
{
    public class GetPageForCloningWithLanguageCommandRequest
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public System.Guid PageId { get; set; }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>
        /// The language id.
        /// </value>
        public System.Guid LanguageId { get; set; }
    }
}