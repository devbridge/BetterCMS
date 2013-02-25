using System;

using Autofac;

using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions;

namespace BetterCms.Core
{
    public class CmsApiContext : IDisposable
    {
        private readonly ILifetimeScope container;

        /// <summary>
        /// Gets the tags API service.
        /// </summary>
        /// <value>
        /// The tags API service.
        /// </value>
        public ITagApiService Tags
        {
            get
            {
                if (container.IsRegistered<ITagApiService>())
                {
                    return container.Resolve<ITagApiService>();
                }

                throw new CmsException("Tags API service was not initialized. Please make sure to add BetterCms.Modules.Root module.");
            }
        }

        /// <summary>
        /// Gets the pages API service.
        /// </summary>
        /// <value>
        /// The pages API service.
        /// </value>
        public IPageApiService Pages
        {
            get
            {
                if (container.IsRegistered<IPageApiService>())
                {
                    return container.Resolve<IPageApiService>();
                }

                throw new CmsException("Pages API service was not initialized. Please make sure to add BetterCms.Modules.Root module.");
            }
        }

        /// <summary>
        /// Gets the layouts API service.
        /// </summary>
        /// <value>
        /// The layouts API service.
        /// </value>
        public ILayoutApiService Layouts
        {
            get
            {
                if (container.IsRegistered<ILayoutApiService>())
                {
                    return container.Resolve<ILayoutApiService>();
                }

                throw new CmsException("Pages API service was not initialized. Please make sure to add BetterCms.Modules.Root module.");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public CmsApiContext(ILifetimeScope container)
        {
            this.container = container;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
