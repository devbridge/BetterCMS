using Autofac;

using BetterCms.Core;
using BetterCms.Core.Exceptions;
using BetterCms.Module.Blog.DataServices;

namespace BetterCms.Module.Blog
{
    public class BlogsApiContext : CmsApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogsApiContext" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public BlogsApiContext(ILifetimeScope container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the blogs API service.
        /// </summary>
        /// <value>
        /// The blogs API service.
        /// </value>
        public IBlogApiService Blogs
        {
            get
            {
                if (container.IsRegistered<IBlogApiService>())
                {
                    return container.Resolve<IBlogApiService>();
                }

                throw new CmsException("Blogs API service was not initialized.");
            }
        }

        /// <summary>
        /// Gets the authors API service.
        /// </summary>
        /// <value>
        /// The authors API service.
        /// </value>
        public IAuthorApiService Authors
        {
            get
            {
                if (container.IsRegistered<IAuthorApiService>())
                {
                    return container.Resolve<IAuthorApiService>();
                }

                throw new CmsException("Authors API service was not initialized.");
            }
        }
    }
}