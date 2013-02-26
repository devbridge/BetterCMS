using Autofac;

using BetterCms.Core;
using BetterCms.Core.Exceptions;
using BetterCms.Module.MediaManager.DataServices;

namespace BetterCms.Module.MediaManager
{
    /// <summary>
    /// Media Manager Module Api Context
    /// </summary>
    public class MediaManagerApiContext : CmsApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaManagerApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MediaManagerApiContext(ILifetimeScope container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the medias API service.
        /// </summary>
        /// <value>
        /// The medias API service.
        /// </value>
        public IMediaApiService Medias
        {
            get
            {
                if (container.IsRegistered<IMediaApiService>())
                {
                    return container.Resolve<IMediaApiService>();
                }

                throw new CmsException("Medias API service was not initialized.");
            }
        }
    }
}