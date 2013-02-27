using Autofac;

using BetterCms.Core;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Navigation.DataServices;

namespace BetterCms.Module.Navigation
{
    /// <summary>
    /// Navigation API context.
    /// </summary>
    public class NavigationApiContext : CmsApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public NavigationApiContext(ILifetimeScope container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the sitemap.
        /// </summary>
        /// <value>
        /// The sitemap.
        /// </value>
        /// <exception cref="CmsException">Sitemap API service was not initialized.</exception>
        public ISitemapApiService Sitemap
        {
            get
            {
                if (container.IsRegistered<ISitemapApiService>())
                {
                    return container.Resolve<ISitemapApiService>();
                }

                throw new CmsException("Sitemap API service was not initialized.");
            }
        }
    }
}