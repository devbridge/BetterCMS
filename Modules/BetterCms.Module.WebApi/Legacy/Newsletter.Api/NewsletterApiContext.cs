using Autofac;

using BetterCms.Core.DataAccess;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Newsletter API Context.
    /// </summary>
    public class NewsletterApiContext : DataApiContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        public NewsletterApiContext(ILifetimeScope lifetimeScope, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
        }
    }
}