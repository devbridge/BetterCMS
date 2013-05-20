using BetterCms.Module.Newsletter.Models;

namespace BetterCms.Module.Newsletter.Services
{
    public interface ISubscriberService
    {
        /// <summary>
        /// Saves the newsletter subscriber.
        /// </summary>
        /// <param name="email">The subscriber email.</param>
        /// <param name="id">The subscriber id.</param>
        /// <param name="version">The version.</param>
        /// /// <param name="ignoreUniqueSubscriberException">if set to <c>true</c> ignore unique subscriber exception.</param>
        /// <returns>
        /// Saved entity
        /// </returns>
        Subscriber SaveSubscriber(string email, System.Guid id, int version, bool ignoreUniqueSubscriberException = false);

        /// <summary>
        /// Saves the subscriber.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// Saved entity
        /// </returns>
        Subscriber SaveSubscriber(string email);
    }
}