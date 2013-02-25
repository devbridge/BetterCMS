namespace BetterCms.Core.Events
{
    /// <summary>
    /// Pages API events
    /// </summary>
    public class CmsApiPagesEvents
    {
        /// <summary>
        /// Occurs when page is created.
        /// </summary>
        public event PageCreatedEventArgs.PageCreatedEventHandler PageCreated;

        /// <summary>
        /// Called when page is created.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PageCreatedEventArgs" /> instance containing the event data.</param>
        public void OnPageCreated(object sender, PageCreatedEventArgs args)
        {
            if (PageCreated != null)
            {
                PageCreated(sender, args);
            }
        }
    }
}
