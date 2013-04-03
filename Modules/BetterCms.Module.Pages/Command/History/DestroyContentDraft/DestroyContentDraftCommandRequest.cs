using System;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommandRequest
    {
        /// <summary>
        /// Gets or sets the destroying draft id.
        /// </summary>
        /// <value>
        /// The the destroying draft id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the destroying draft version.
        /// </summary>
        /// <value>
        /// The destroying draft version.
        /// </value>
        public int Version { get; set; }
    }
}