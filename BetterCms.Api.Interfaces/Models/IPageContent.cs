namespace BetterCms.Api.Interfaces.Models
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
    }
}
