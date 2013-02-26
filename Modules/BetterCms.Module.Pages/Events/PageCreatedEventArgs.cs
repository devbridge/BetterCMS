using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Events
{
    /// <summary>
    /// Page Created Event Arguments
    /// </summary>
    public class PageCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageCreatedEventArgs" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public PageCreatedEventArgs(PageProperties page)
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

        /// <summary>
        /// Delegate, handles page creation event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PageCreatedEventArgs" /> instance containing the event data.</param>
        public delegate void PageCreatedEventHandler(object sender, PageCreatedEventArgs args);
    }
}
