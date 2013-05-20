using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using BetterCms.Module.Pages.Api.Dto;

using NHibernate.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

// ReSharper disable CheckNamespace
namespace BetterCms.Api
// ReSharper restore CheckNamespace
{
    public partial class PagesApiContext
    {
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
                return Repository
                        .AsQueryable<Layout>()
                        .ApplyFilters(true, filter, order, orderDescending, pageNumber, itemsPerPage)
                        .FetchMany(l => l.LayoutRegions)
                        .ThenFetch(l => l.Region)
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

                return Repository.AsQueryable(filter, order, orderDescending, pageNumber, itemsPerPage).ToList();
            }
            catch (Exception inner)
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

                return Repository
                    .AsQueryable<LayoutRegion>()
                    .Where(lr => lr.Layout.Id == layoutId)
                    .ApplyFilters(filter, order, orderDescending, pageNumber, itemsPerPage)
                    .Fetch(lr => lr.Region)
                    .ToList();
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get layout regions list for layout Id={0}.", layoutId);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the layout.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Created layout entity
        /// </returns>
        public Layout CreateLayout(CreateLayoutRequest request)
        {
            if (!HttpHelper.VirtualPathExists(request.LayoutPath))
            {
                var message = string.Format("Failed to create layout: layout by given path {0} doesn't exist.", request.LayoutPath);
                Logger.Error(message);
                throw new CmsApiValidationException(message);
            }

            try
            {
                UnitOfWork.BeginTransaction();

                var layout = new Layout
                {
                    LayoutPath = request.LayoutPath,
                    Name = request.Name,
                    PreviewUrl = request.PreviewUrl
                };

                // reference or create new regions by identifiers
                if (request.Regions != null)
                {
                    layout.LayoutRegions = new List<LayoutRegion>();
                    foreach (var regionIdentifier in request.Regions)
                    {
                        if (string.IsNullOrWhiteSpace(regionIdentifier))
                        {
                            continue;
                        }

                        var region = LoadOrCreateRegion(regionIdentifier);

                        var layoutRegion = new LayoutRegion
                        {
                            Layout = layout,
                            Region = region
                        };
                        layout.LayoutRegions.Add(layoutRegion);
                    }
                }

                Repository.Save(layout);
                UnitOfWork.Commit();

                return layout;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create layout. Path: {0}, Name: {1}, Url: {2}", request.LayoutPath, request.Name, request.PreviewUrl);
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Creates the layout region.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Created layout region entity
        /// </returns>
        public LayoutRegion CreateLayoutRegion(CreateLayoutRegionRequest request)
        {
            try
            {
                var layout = Repository.AsProxy<Layout>(request.LayoutId);
                var region = LoadOrCreateRegion(request.RegionIdentifier);

                if (!region.Id.HasDefaultValue())
                {
                    var exists = Repository.AsQueryable<LayoutRegion>(lr => lr.Region == region && lr.Layout == layout).Any();
                    if (exists)
                    {
                        var message = string.Format("Failed to create layout region: region {0} is already assigned.", request.RegionIdentifier);
                        var logMessage = string.Format("{0} LayoutId: {1}", message, request.LayoutId);
                        Logger.Error(logMessage);
                        throw new CmsApiValidationException(message);
                    }
                }

                var layoutRegion = new LayoutRegion { Layout = layout, Region = region };
                Repository.Save(layoutRegion);

                UnitOfWork.Commit();

                return layoutRegion;
            }
            catch (CmsApiValidationException)
            {
                throw;
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to create layout region.");
                Logger.Error(message, inner);
                throw new CmsApiException(message, inner);
            }
        }

        /// <summary>
        /// Loads the or creates the region.
        /// </summary>
        /// <returns>Region entity</returns>
        private Region LoadOrCreateRegion(string regionIdentifier)
        {
            var region = Repository
                            .AsQueryable<Region>(r => r.RegionIdentifier == regionIdentifier)
                            .FirstOrDefault();

            if (region == null)
            {
                region = new Region { RegionIdentifier = regionIdentifier };
            }

            return region;
        }
    }
}