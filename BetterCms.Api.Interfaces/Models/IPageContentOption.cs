namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access the page content option.
    /// </summary>
    public interface IPageContentOption : IOption
    {
        IPageContent PageContent { get; }        
    }
}
