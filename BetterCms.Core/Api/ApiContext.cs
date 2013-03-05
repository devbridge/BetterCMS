using System;

using Autofac;

using BetterCms.Core.Exceptions;

using Common.Logging;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public abstract class ApiContext : IDisposable
    {
        /// <summary>
        /// The current class logger.
        /// </summary>
        protected static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The lifetime scope of current API context.
        /// </summary>
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Marker to check if this API context has parent context scope.
        /// </summary>
        private bool hasParentContextScope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The container.</param>
        protected ApiContext(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        /// <summary>
        /// Resolves an instance of TService type from given lifetime scope.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>TService object.</returns>
        /// <exception cref="CmsException">Thrown if TService type was not found in lifetime scope.</exception>
        protected TService Resolve<TService>()
        {

            if (lifetimeScope.IsRegistered<TService>())
            {
                return lifetimeScope.Resolve<TService>();
            }

            throw new CmsException(string.Format("A {0} is unknown type in the API context.", typeof(TService).Name));
        }

        /// <summary>
        /// Gets the lifetime scope.
        /// </summary>
        /// <returns></returns>
        internal ILifetimeScope GetLifetimeScope()
        {
            return lifetimeScope;
        }

        /// <summary>
        /// Marks current API context with parent lifetime scope.
        /// </summary>
        internal void MarkParentLifetimeScope()
        {
            hasParentContextScope = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {            
            if (lifetimeScope != null && !hasParentContextScope)
            {
                lifetimeScope.Dispose();
            }
        }
    }
}
