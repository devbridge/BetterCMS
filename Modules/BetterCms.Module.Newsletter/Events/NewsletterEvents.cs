using BetterCms.Module.Newsletter.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attachable newsletter events container
    /// </summary>
    public class NewsletterEvents : EventsBase<NewsletterEvents>
    {
        /// <summary>
        /// Occurs when a subscriber is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Subscriber>> SubscriberCreated;

        /// <summary>
        /// Occurs when a subscriber is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Subscriber>> SubscriberUpdated;

        /// <summary>
        /// Occurs when a subscriber is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Subscriber>> SubscriberDeleted;

        /// <summary>
        /// Called when subscriber is created.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void OnSubscriberCreated(Subscriber subscriber)
        {
            if (SubscriberCreated != null)
            {
                SubscriberCreated(new SingleItemEventArgs<Subscriber>(subscriber));
            }
        }
        
        /// <summary>
        /// Called when subscriber is updates.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void OnSubscriberUpdated(Subscriber subscriber)
        {
            if (SubscriberUpdated != null)
            {
                SubscriberUpdated(new SingleItemEventArgs<Subscriber>(subscriber));
            }
        }

        /// <summary>
        /// Called when subscriber is deleted.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        public void OnSubscriberDeleted(Subscriber subscriber)
        {
            if (SubscriberDeleted != null)
            {
                SubscriberDeleted(new SingleItemEventArgs<Subscriber>(subscriber));
            }
        }
    }
}
