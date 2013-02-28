using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataServices;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.DataServices
{
    public class DefaultLayoutApiService : ApiServiceBase, ILayoutApiService
    {
        private IRepository repository { get; set; }
        private IUnitOfWork unitOfWork { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLayoutApiService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DefaultLayoutApiService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the list of layout entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of layout entities
        /// </returns>
        public IList<Layout> GetLayouts(Expression<Func<Layout, bool>> filter = null, Expression<Func<Layout, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                return repository
                    .AsQueryable<Layout>()
                    .FetchMany(l => l.LayoutRegions)
                    .ThenFetch(l => l.Region)
                    .ApplyFilters(true, filter, order, orderDescending, pageNumber, itemsPerPage)
                    .ToList();

                /* TODO: remove or use
                 * WORKS GREAT WITHOUT METHODS IN WHERE CLAUSE (SUCH AS Contains)
                 * LayoutRegion lrAlias = null;
                Region rAlias = null;

                return unitOfWork.Session
                    .QueryOver<Layout>()
                    .JoinAlias(l => l.LayoutRegions, () => lrAlias, JoinType.LeftOuterJoin)
                    .JoinAlias(l => lrAlias.Region, () => rAlias, JoinType.LeftOuterJoin)
                    .ApplySubQueryFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                    .List();
                 */
            }
            catch (Exception inner)
            {
                const string message = "Failed to get layouts list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }


        /// <summary>
        /// Gets the list of region entities.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of region entities
        /// </returns>
        public IList<Region> GetRegions(Expression<Func<Region, bool>> filter = null, Expression<Func<Region, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.RegionIdentifier;
                }

                return repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch(Exception inner)
            {
                const string message = "Failed to get regions list.";
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Gets the list of specified layout region entities.
        /// </summary>
        /// <param name="layoutId">The layout id.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="order">The order.</param>
        /// <param name="orderDescending">if set to <c>true</c> order by descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The items per page.</param>
        /// <returns>
        /// The list of specified layout region entities
        /// </returns>
        public IList<LayoutRegion> GetLayoutRegions(Guid layoutId, Expression<Func<LayoutRegion, bool>> filter = null, Expression<Func<LayoutRegion, dynamic>> order = null, bool orderDescending = false, int? pageNumber = null, int? itemsPerPage = null)
        {
            try
            {
                if (order == null)
                {
                    order = p => p.Description;
                }

                return repository
                    .AsQueryable<LayoutRegion>()
                    .Fetch(lr => lr.Region)
                    .Where(lr => lr.Layout.Id == layoutId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get layout regions list for layout Id={0}.", layoutId);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }
    }
}