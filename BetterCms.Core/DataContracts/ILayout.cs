using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic layout properties.
    /// </summary>
    public interface ILayout : IEntity
    {
        /// <summary>
        /// Gets the layout name.
        /// </summary>
        /// <value>
        /// The layout name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the layout URL.
        /// </summary>
        /// <value>
        /// The layout URL.
        /// </value>
        string LayoutPath { get; }

        /// <summary>
        /// Gets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        IList<IRegion> Regions { get; }
    }
}
