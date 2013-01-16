namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access a basic page content properties.
    /// </summary>
    public interface IPageContent : IEntity
    {
        int Order { get; }

        IPage Page { get; }

        IContent Content { get; }

        IRegion Region { get; }

        /// <summary>
        /// Gets a value indicating this content publishing status (auto saved for preview, draft, published, archived).
        /// </summary>        
        ContentStatus Status { get; }
    }
}
