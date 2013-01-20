using System;

namespace BetterCms.Module.Pages.Command.Base
{
    public interface ISaveContentResponse
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        Guid ContentId { get; set; }
    }
}
