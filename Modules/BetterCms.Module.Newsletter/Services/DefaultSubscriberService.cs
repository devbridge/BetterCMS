// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultSubscriberService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Exceptions;
using BetterCms.Module.Newsletter.Models;

using BetterCms.Module.Root.Mvc;

using Common.Logging;

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
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

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
            if (!ValidateSubscriber(id, email, out subscriber))
            {
                var logMessage = string.Format("Subscriber with entered Email {0} already is subscribed.", email);

                if (!ignoreUniqueSubscriberException)
                {
                    var message = string.Format(NewsletterGlobalization.SaveSubscriberCommand_EmailAlreadyExists_Message, email);
                    throw new UniqueSubscriberException(() => message, logMessage);
                }

                Logger.Info(logMessage);

                return subscriber;
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
                Events.NewsletterEvents.Instance.OnSubscriberCreated(subscriber);
            }
            else
            {
                Events.NewsletterEvents.Instance.OnSubscriberUpdated(subscriber);
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
        /// <param name="subscriber">The subscriber.</param>
        /// <returns></returns>
        private bool ValidateSubscriber(Guid id, string email, out Subscriber subscriber)
        {
            var query = repository.AsQueryable<Subscriber>(s => s.Email == email);
            if (!id.HasDefaultValue())
            {
                query = query.Where(s => s.Id != id);
            }

            subscriber = query.FirstOrDefault();
            return subscriber == null;
        }
    }
}