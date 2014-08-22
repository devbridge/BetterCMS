using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic content properties.
    /// </summary>
    public interface IContent : IEntity, IHistorical
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

        /// <summary>
        /// Gets the list of child content.
        /// </summary>
        /// <value>
        /// The child contents.
        /// </value>
        IList<IChildContent> Children { get; }
    }
}
