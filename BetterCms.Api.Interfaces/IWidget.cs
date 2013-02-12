namespace BetterCms.Api.Interfaces.Models
{
    /// <summary>
    /// Defines interface to access basic widget properties.
    /// </summary>
    public interface IWidget : IContent
    {
        ICategory Category { get; }
    }
}
