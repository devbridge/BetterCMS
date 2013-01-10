namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IContent : IEntity
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the preview URL.
        /// </summary>
        /// <value>
        /// The preview URL.
        /// </value>
        string PreviewUrl { get; }
    }
}
