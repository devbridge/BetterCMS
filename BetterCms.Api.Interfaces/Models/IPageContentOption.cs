namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access the page content option.
    /// </summary>
    public interface IPageContentOption : IOption
    {
        IPageContent PageContent { get; }        
    }
}
