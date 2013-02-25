namespace BetterCms.Core.Events
{
    /// <summary>
    /// Attachable events container
    /// </summary>
    public class CmsApiEvents
    {
        private static CmsApiEvents instance;

        private CmsApiPagesEvents pages;

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiEvents" /> class.
        /// </summary>
        protected CmsApiEvents()
        {
        }

        /// <summary>
        /// Gets the static singletone instance.
        /// </summary>
        /// <value>
        /// The static singletone instance.
        /// </value>
        public static CmsApiEvents Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CmsApiEvents();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets the pages events container.
        /// </summary>
        /// <value>
        /// The pages events container.
        /// </value>
        public CmsApiPagesEvents Pages
        {
            get
            {
                if (pages == null)
                {
                    pages = new CmsApiPagesEvents();
                }

                return pages;
            }
        }
    }
}
