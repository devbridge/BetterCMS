using System;
using System.Collections.Generic;
using System.Linq;

using NHibernate.Linq;

using BetterCms.Core.Api.DataContracts;
using BetterCms.Core.Api.Extensions;
using BetterCms.Core.Exceptions.Api;

using BetterCms.Module.Pages.Api.DataContracts;
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
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of layout entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Layout> GetLayouts(GetLayoutsRequest request = null)
        {
            try
            {
                var result = Repository
                        .AsQueryable<Layout>()
                        .ApplyFiltersWithChildren(request);
                
                var query = result.Item1;
                var totalCount = result.Item2;

                query = query
                        .AddOrder(request)
                        .FetchMany(l => l.LayoutRegions)
                        .ThenFetch(l => l.Region);

                return query.ToDataListResponse(totalCount);
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
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of region entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<Region> GetRegions(GetRegionsRequest request = null)
        {
            try
            {
                return Repository.ToDataListResponse(request);
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
        /// <param name="request">The request.</param>
        /// <returns>
        /// The list of specified layout region entities
        /// </returns>
        /// <exception cref="CmsApiException"></exception>
        public DataListResponse<LayoutRegion> GetLayoutRegions(GetLayoutRegionsRequest request)
        {
            try
            {
                var query = Repository
                    .AsQueryable<LayoutRegion>()
                    .Where(lr => lr.Layout.Id == request.LayoutId)
                    .ApplyFilters(request);

                var totalCount = query.ToRowCountFutureValue(request);

                query = query
                   .AddOrderAndPaging(request)
                   .Fetch(lr => lr.Region);

                return query.ToDataListResponse(totalCount);
            }
            catch (Exception inner)
            {
                var message = string.Format("Failed to get layout regions list for layout Id={0}.", request.LayoutId);
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
            ValidateRequest(request);

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
            ValidateRequest(request);

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