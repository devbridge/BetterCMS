namespace BetterCms.Module.Pages.Events
{
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public class PagesEvents
    {
        private static PagesEvents instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesEvents" /> class.
        /// </summary>
        protected PagesEvents()
        {
        }

        /// <summary>
        /// Gets the static singletone instance.
        /// </summary>
        /// <value>
        /// The static singletone instance.
        /// </value>
        public static PagesEvents Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PagesEvents();
                }

                return instance;
            }
        }

        /// <summary>
        /// Occurs when page is created.
        /// </summary>
        public event PageCreatedEventArgs.PageCreatedEventHandler PageCreated;
        
        /// <summary>
        /// Occurs when page is deleted.
        /// </summary>
        public event PageDeletedEventArgs.PageDeletedEventHandler PageDeleted;

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

        /// <summary>
        /// Called when page is deleted.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="PageDeletedEventArgs" /> instance containing the event data.</param>
        public void OnPageDeleted(object sender, PageDeletedEventArgs args)
        {
            if (PageDeleted != null)
            {
                PageDeleted(sender, args);
            }
        }
    }
}
