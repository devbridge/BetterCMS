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

        /// <summary>
        /// Determines, if child regions should be included to the results.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child regions should be included to the results; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeChildRegions { get; set; }
    }
}