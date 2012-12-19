using System;

namespace BetterCms.Module.Root.Mvc.Grids
{
    /// <summary>
    /// Interface for grid editable item
    /// </summary>
    public interface IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; set; }
    }
}