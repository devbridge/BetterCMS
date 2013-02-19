using System;

using Autofac;

using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions;

namespace BetterCms.Core
{
    public class CmsApiContext : IDisposable
    {
        private readonly ILifetimeScope container;

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

        public IPageApiService Pages
        {
            get
            {
                if (container.IsRegistered<IPageApiService>())
                {
                    return container.Resolve<IPageApiService>();
                }

                throw new CmsException("Tags API service was not initialized. Please make sure to add BetterCms.Modules.Root module.");
            }
        }

        public CmsApiContext(ILifetimeScope container)
        {
            this.container = container;
        }

        public void Dispose()
        {
            // TODO: what dispose ????
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
