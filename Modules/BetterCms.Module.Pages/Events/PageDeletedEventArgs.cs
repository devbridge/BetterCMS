using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Events
{
    /// <summary>
    /// Page Deleted Event Arguments
    /// </summary>
    public class PageDeletedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageDeletedEventArgs" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public PageDeletedEventArgs(PageProperties page)
        {
            Page = page;
        }

        /// <summary>
        /// Gets or sets the deleted page.
        /// </summary>
        /// <value>
        /// The deleted page.
        /// </value>
        public PageProperties Page { get; set; }

        /// <summary>
        /// Delegate, handles page deletion event
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PageDeletedEventArgs" /> instance containing the event data.</param>
        public delegate void PageDeletedEventHandler(object sender, PageDeletedEventArgs args);
    }
}
