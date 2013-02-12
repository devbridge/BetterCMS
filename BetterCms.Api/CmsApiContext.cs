using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autofac;

using BetterCms.Api.Services;

namespace BetterCms.Api
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
                return pageApiService;
            }
        }

        public CmsApiContext(ILifetimeScope container)
        {
            this.container = container;
        }
    }
}
