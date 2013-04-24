using Autofac;

using BetterCms.Core.DataAccess;
using BetterCms.Module.Newsletter.Api.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Newsletter API Context.
    /// </summary>
    public class NewsletterApiContext : DataApiContext
    {
        private static readonly NewsletterEvents events;

        /// <summary>
        /// Initializes the <see cref="NewsletterApiContext" /> class.
        /// </summary>
        static NewsletterApiContext()
        {
            events = new NewsletterEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        public NewsletterApiContext(ILifetimeScope lifetimeScope, IRepository repository = null)
            : base(lifetimeScope, repository)
        {
        }

        /// <summary>
        /// Gets the newsletter events.
        /// </summary>
        /// <value>
        /// The newsletter events.
        /// </value>
        public new static NewsletterEvents Events
        {
            get
            {
                return events;
            }
        }
    }
}