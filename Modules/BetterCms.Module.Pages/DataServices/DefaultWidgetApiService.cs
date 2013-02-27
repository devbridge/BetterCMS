using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.DataServices
{
    /// <summary>
    /// Widgets API service implementation.
    /// </summary>
    public class DefaultWidgetApiService : ApiServiceBase, IWidgetApiService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWidgetApiService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultWidgetApiService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the HTML content widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Html widget.</returns>
        /// <exception cref="CmsApiException">Failed to get html content widget.</exception>
        public HtmlContentWidget GetHtmlContentWidget(Guid id)
        {
            try
            {
                return repository.First<HtmlContentWidget>(w => w.Id == id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get html content widget by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the server control widget.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Server widget.</returns>
        /// <exception cref="CmsApiException">Failed to get server control widget.</exception>
        public ServerControlWidget GetServerControlWidget(Guid id)
        {
            try
            {
                return repository.First<ServerControlWidget>(w => w.Id == id);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get server control widget by id={0}.", id);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the widgets.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> [order descending].</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>Widget list.</returns>
        /// <exception cref="CmsApiException">Failed to get widgets.</exception>
        public IList<Widget> GetWidgets(Expression<Func<Widget, bool>> filter = null, Expression<Func<Widget, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Name;
                }

                return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get widgets.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the page widgets.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Widget list.</returns>
        /// <exception cref="CmsApiException">Failed to get widgets.</exception>
        public IList<Widget> GetPageWidgets(Guid pageId, Expression<Func<Widget, bool>> filter = null)
        {
            try
            {
                var query = repository
                    .AsQueryable<Widget>()
                    .Where(w => w.PageContents.Any(c => c.Page.Id == pageId));

                if (filter != null)
                {
                    query = query.Where(filter);
                }
                
                return query.ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get page widgets.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}