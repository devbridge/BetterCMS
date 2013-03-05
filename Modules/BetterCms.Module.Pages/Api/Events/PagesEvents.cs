using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Api.Events
{
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public class PagesEvents
    {
        /// <summary>
        /// Delegate to handle a page creation event.
        /// </summary>
        /// <param name="args">The <see cref="PagePropertiesEventArgs" /> instance containing the event data.</param>
        public delegate void PageCreatedEventHandler(PagePropertiesEventArgs args);

        /// <summary>
        /// Delegate to handle a page deletion event.
        /// </summary>
        /// <param name="args">The <see cref="PagePropertiesEventArgs" /> instance containing the event data.</param>
        public delegate void PageDeletedEventHandler(PagePropertiesEventArgs args);

        /// <summary>
        /// Occurs when page is created.
        /// </summary>
        public event PageCreatedEventHandler PageCreated;
        
        /// <summary>
        /// Occurs when page is deleted.
        /// </summary>
        public event PageDeletedEventHandler PageDeleted;

        /// <summary>
        /// Called when page is created.
        /// </summary>
        public void OnPageCreated(PageProperties page)
        {
            if (PageCreated != null)
            {
                PageCreated(new PagePropertiesEventArgs(page));
            }
        }

        /// <summary>
        /// Called when page is deleted.
        /// </summary>
        public void OnPageDeleted(PageProperties page)
        {
            if (PageDeleted != null)
            {
                PageDeleted(new PagePropertiesEventArgs(page));
            }
        }
    }
}
