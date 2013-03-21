using System;
using System.Web;

using Autofac;

using BetterCms.Core.Web;

namespace BetterCms.Core.Dependencies
{
    /// <summary>
    /// Service locator to support per web request life time manager.
    /// </summary>
    public class PerWebRequestContainerProvider
    {      
        /// <summary>
        /// Marker key.
        /// </summary>
        private static readonly object PerWebRequestContainerKey = new object();

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PerWebRequestContainerProvider" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public PerWebRequestContainerProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the child container.
        /// </summary>
        /// <value>
        /// The child container.
        /// </value>
        public ILifetimeScope CurrentScope
        {
            get
            {
                var httpContext = httpContextAccessor.GetCurrent();

                ILifetimeScope requestContainer = httpContext.Items[PerWebRequestContainerKey] as ILifetimeScope;
                
                if (requestContainer == null)
                {
                    httpContext.Items[PerWebRequestContainerKey] = requestContainer = ContextScopeProvider.CreateChildContainer();
                }

                return requestContainer;
            }
        }

        /// <summary>
        /// Gets the lifetime scope.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The child container.</returns>
        public static ILifetimeScope GetLifetimeScope(HttpContextBase context)
        {
            return context.Items[PerWebRequestContainerKey] as ILifetimeScope;
        }

        /// <summary>
        /// Disposes the managed resources.
        /// </summary>
        protected void DisposeManagedResources()
        {
            var httpContext = httpContextAccessor.GetCurrent();
            if (httpContext != null)
            {
                var requestContainer = httpContext.Items[PerWebRequestContainerKey] as ILifetimeScope;

                if (requestContainer != null)
                {
                    requestContainer.Dispose();
                }
            }
        }

        /// <summary>
        /// Disposes the current scope.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        internal static void DisposeCurrentScope(object sender, EventArgs args)
        {
            var httpApplication = sender as HttpApplication;
            if (httpApplication != null)
            {
                if (httpApplication.Context != null)
                {
                    var requestContainer = httpApplication.Context.Items[PerWebRequestContainerKey] as ILifetimeScope;
                    if (requestContainer != null)
                    {
                        requestContainer.Dispose();
                    }
                }
            }
        }
    }
}