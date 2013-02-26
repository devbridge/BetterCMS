using Autofac;

using BetterCms.Core;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Pages.DataServices;
using BetterCms.Module.Pages.Events;

namespace BetterCms.Module.Pages
{
    public class PagesApiContext : CmsApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagesApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public PagesApiContext(ILifetimeScope container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public static PagesEvents Events
        {
            get
            {
                return PagesEvents.Instance;
            }
        }

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

                throw new CmsException("Tags API service was not initialized.");
            }
        }
        
        /// <summary>
        /// Gets the redirects API service.
        /// </summary>
        /// <value>
        /// The redirects API service.
        /// </value>
        public IRedirectApiService Redirects
        {
            get
            {
                if (container.IsRegistered<IRedirectApiService>())
                {
                    return container.Resolve<IRedirectApiService>();
                }

                throw new CmsException("Redirects API service was not initialized.");
            }
        }

        /// <summary>
        /// Gets the categories API service.
        /// </summary>
        /// <value>
        /// The categories API service.
        /// </value>
        public ICategoryApiService Categories
        {
            get
            {
                if (container.IsRegistered<ICategoryApiService>())
                {
                    return container.Resolve<ICategoryApiService>();
                }

                throw new CmsException("Categories API service was not initialized.");
            }
        }

        /// <summary>
        /// Gets the contents API service.
        /// </summary>
        /// <value>
        /// The contents API service.
        /// </value>
        public IContentApiService Contents
        {
            get
            {
                if (container.IsRegistered<IContentApiService>())
                {
                    return container.Resolve<IContentApiService>();
                }

                throw new CmsException("Contents API service was not initialized.");
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

                throw new CmsException("Layouts API service was not initialized.");
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

                throw new CmsException("Pages API service was not initialized.");
            }
        }

        /// <summary>
        /// Gets the widgets API service.
        /// </summary>
        /// <value>
        /// The widgets API service.
        /// </value>
        public IWidgetApiService Widgets
        {
            get
            {
                if (container.IsRegistered<IWidgetApiService>())
                {
                    return container.Resolve<IWidgetApiService>();
                }

                throw new CmsException("Widgets API service was not initialized.");
            }
        }
    }
}