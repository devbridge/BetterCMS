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
        /// Gets or sets the culture group identifier.
        /// </summary>
        /// <value>
        /// The culture group identifier.
        /// </value>
        public virtual System.Guid? CultureGroupIdentifier { get; set; }

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

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id: {1}, Title: {2}, PageUrl: {3}, CultureId: {4}, CultureGroupIdentifier: {5}", 
                base.ToString(), Id, Title, PageUrl, CultureId, CultureGroupIdentifier);
        }
    }
}