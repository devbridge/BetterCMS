using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Pages.Api.Events;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public partial class PagesApiContext : DataApiContext
    {
        private static readonly PagesApiEvents events;

        /// <summary>
        /// The sitemap service.
        /// </summary>
        private readonly ISitemapService sitemapService;

        /// <summary>
        /// The history service.
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// Initializes the <see cref="PagesApiContext" /> class.
        /// </summary>
        static PagesApiContext()
        {
            events = new PagesApiEvents();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesApiContext" /> class.
        /// </summary>
        /// <param name="lifetimeScope">The lifetime scope.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="sitemapService">The sitemap service.</param>
        /// <param name="historyService">The history service.</param>
        public PagesApiContext(ILifetimeScope lifetimeScope, IRepository repository = null, ISitemapService sitemapService = null, IHistoryService historyService = null)
            : base(lifetimeScope, repository)
        {
            if (historyService == null)
            {
                this.historyService = Resolve<IHistoryService>();
            }
            else
            {
                this.historyService = historyService;
            }

            if (sitemapService == null)
            {
                this.sitemapService = Resolve<ISitemapService>();
            }
            else
            {
                this.sitemapService = sitemapService;
            }
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        /// <value>
        /// The events.
        /// </value>
        public new static PagesApiEvents Events
        {
            get
            {
                return events;
            }
        }

        /// <summary>
        /// Gets the tags.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of tag entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IList<Tag> GetTags(GetDataRequest<Tag> request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetDataRequest<Tag>();
                }
                request.SetDefaultOrder(p => p.Name);

                return Repository.AsQueryable(request).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get tags list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of redirect entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of redirect entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IList<Redirect> GetRedirects(GetDataRequest<Redirect> request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetDataRequest<Redirect>();
                }
                request.SetDefaultOrder(p => p.PageUrl);

                return Repository.AsQueryable(request).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get redirects list.";
                Logger.Error(message, inner);

                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of category entities.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of category entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public IList<Category> GetCategories(GetDataRequest<Category> request = null)
        {
            try
            {
                if (request == null)
                {
                    request = new GetDataRequest<Category>();
                }
                request.SetDefaultOrder(c => c.Name);

                return Repository.AsQueryable(request).ToList();
            }
            catch (Exception inner)
            {
                const string message = "Failed to get categories list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}