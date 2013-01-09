namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access the page content option.
    /// </summary>
    public interface IPageContentOption : IEntity
    {
        IPageContent PageContent { get; }

        IContentOption ContentOption { get; }

        string Value { get; }
    }
}
