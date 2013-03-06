using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.Events
{
    /// <summary>
    /// Page Created Event Arguments
    /// </summary>
    public class PagePropertiesEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagePropertiesEventArgs" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public PagePropertiesEventArgs(PageProperties page)
        {
            Page = page;
        }

        /// <summary>
        /// Gets or sets the created page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public PageProperties Page { get; set; }
    }
}
