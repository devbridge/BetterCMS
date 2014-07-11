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
        /// Gets or sets the page URL hash.
        /// </summary>
        /// <value>
        /// The page URL hash.
        /// </value>
        internal string PageUrlHash { get; set; }

        /// <summary>
        /// Gets or sets the page language id.
        /// </summary>
        /// <value>
        /// The page language id.
        /// </value>
        public virtual System.Guid? LanguageId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Title: {2}, PageUrl: {3}, LanguageId: {4}", 
                base.ToString(), Id, Title, PageUrl, LanguageId);
        }
    }
}