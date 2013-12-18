namespace BetterCMS.Module.LuceneSearch.Services.WebCrawlerService
{
    public interface IWebCrawlerService
    {
        PageData FetchPage(string url);
    }
}