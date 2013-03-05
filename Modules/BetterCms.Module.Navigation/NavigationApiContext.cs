using Autofac;

using BetterCms.Core.Api;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Navigation.DataServices;

namespace BetterCms.Module.Navigation
{
    /// <summary>
    /// Navigation API context.
    /// </summary>
    public class NavigationApiContext : DataApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The container.</param>
        public NavigationApiContext(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
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
                return Resolve<ISitemapApiService>();                
            }
        }
    }
}