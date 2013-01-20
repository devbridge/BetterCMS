using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Command.Base
{
    public interface ISaveContentHistory
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        int Version { get; set; }

        /// <summary>
        /// Gets or sets the desirable status.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        ContentStatus DesirableStatus { get; set; }
    }
}
