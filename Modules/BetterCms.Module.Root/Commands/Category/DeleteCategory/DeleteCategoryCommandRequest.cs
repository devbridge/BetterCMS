using System;

namespace BetterCms.Module.Root.Commands.Category.DeleteCategory
{
    /// <summary>
    /// Data contract for DeleteCategoryCommand.
    /// </summary>
    public class DeleteCategoryCommandRequest
    {
        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}