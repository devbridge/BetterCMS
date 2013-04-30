using System;
using System.Linq;

using BetterCms.Api;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;

using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Exceptions;
using BetterCms.Module.Newsletter.Models;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Newsletter.Services
{
    public class DefaultSubscriberService : ISubscriberService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSubscriberService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultSubscriberService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Saves the newsletter subscriber.
        /// </summary>
        /// <param name="email">The subscriber email.</param>
        /// <param name="id">The subscriber id.</param>
        /// <param name="version">The version.</param>
        /// <param name="ignoreUniqueSubscriberException">if set to <c>true</c> ignore unique subscriber exception.</param>
        /// <returns>
        /// Saved entity
        /// </returns>
        public Subscriber SaveSubscriber(string email, Guid id, int version, bool ignoreUniqueSubscriberException = false)
        {
            var isNew = id.HasDefaultValue();
            Subscriber subscriber;

            // Validate
            try
            {
                ValidateSubscriber(id, email);
            }
            catch (UniqueSubscriberException)
            {
                if (!ignoreUniqueSubscriberException)
                {
                    throw;
                }
            }

            if (isNew)
            {
                subscriber = new Subscriber();
            }
            else
            {
                subscriber = repository.AsQueryable<Subscriber>(w => w.Id == id).FirstOne();
            }

            subscriber.Email = email;
            subscriber.Version = version;

            repository.Save(subscriber);
            unitOfWork.Commit();

            if (isNew)
            {
                NewsletterApiContext.Events.OnSubscriberCreated(subscriber);
            }
            else
            {
                NewsletterApiContext.Events.OnSubscriberUpdated(subscriber);
            }

            return subscriber;
        }

        /// <summary>
        /// Saves the subscriber.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>
        /// Saved entity
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Subscriber SaveSubscriber(string email)
        {
            return SaveSubscriber(email, Guid.Empty, 0);
        }

        /// <summary>
        /// Validates the subscriber.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="email">The email.</param>
        private void ValidateSubscriber(Guid id, string email)
        {
            var query = repository.AsQueryable<Subscriber>(s => s.Email == email);
            if (!id.HasDefaultValue())
            {
                query = query.Where(s => s.Id != id);
            }

            if (query.Select(s => s.Id).Any())
            {
                var logMessage = string.Format("Subscriber with entered Email {0} already is subscribed.", email);
                var message = string.Format(NewsletterGlobalization.SaveSubscriberCommand_EmailAlreadyExists_Message, email);
                throw new UniqueSubscriberException(() => message, logMessage);
            }
        }
    }
}