namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class PageTranslationViewModel
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        /// <value>
        /// The page id.
        /// </value>
        public virtual System.Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the main culture page id.
        /// </summary>
        /// <value>
        /// The main culture page id.
        /// </value>
        public virtual System.Guid? MainCulturePageId { get; set; }

        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public virtual string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the page culture id.
        /// </summary>
        /// <value>
        /// The page culture id.
        /// </value>
        public virtual System.Guid? CultureId { get; set; }
    }
}