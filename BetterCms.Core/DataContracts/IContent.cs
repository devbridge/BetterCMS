using System.Collections.Generic;

using BetterModules.Core.DataContracts;

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
        /// Gets the list of content regions.
        /// </summary>
        /// <value>
        /// The list of content regions.
        /// </value>
        IEnumerable<IContentRegion> ContentRegions { get; }
    }
}
