namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access basic widget properties.
    /// </summary>
    public interface IWidget : IContent
    {
        ICategory Category { get; }
    }
}
