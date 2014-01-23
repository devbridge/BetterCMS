namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public interface IWebCrawlerService
    {
        /// <summary>
        /// Fetches the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        /// Fetched page data
        /// </returns>
        PageData FetchPage(string url);

        /// <summary>
        /// Gets a value indicating whether web crawler is correctly configured.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <value>
        ///   <c>true</c> if web crawler is correctly configured; otherwise, <c>false</c>.
        ///   </value>
        bool IsConfigured(out string message);
    }
}