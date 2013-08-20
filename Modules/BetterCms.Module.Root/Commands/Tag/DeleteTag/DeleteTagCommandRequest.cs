using System;

namespace BetterCms.Module.Root.Commands.Tag.DeleteTag
{
    /// <summary>
    /// Data contract for DeleteTagCommand.
    /// </summary>
    public class DeleteTagCommandRequest
    {
        /// <summary>
        /// Gets or sets the tag id.
        /// </summary>
        /// <value>
        /// The tag id.
        /// </value>
        public Guid TagId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}