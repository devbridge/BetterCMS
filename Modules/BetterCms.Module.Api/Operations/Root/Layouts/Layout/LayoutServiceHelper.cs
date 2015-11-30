// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutServiceHelper.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout.Regions;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout
{
    public static class LayoutServiceHelper
    {
        public static IList<OptionModel> GetLayoutOptionsList(IRepository repository, Guid layoutId)
        {
            return GetLayoutOptionsQuery(repository, layoutId).ToList();
        }

        public static DataListResponse<OptionModel> GetLayoutOptionsResponse(IRepository repository, Guid layoutId, RequestBase<DataOptions> request)
        {
            return GetLayoutOptionsQuery(repository, layoutId).ToDataListResponse(request);
        }

        private static IQueryable<OptionModel> GetLayoutOptionsQuery(IRepository repository, Guid layoutId)
        {
            return repository
                .AsQueryable<LayoutOption>(o => o.Layout.Id == layoutId)
                .Select(o => new OptionModel
                    {
                        Key = o.Key,
                        DefaultValue = o.DefaultValue,
                        Type = (OptionType)(int)o.Type,
                        CustomTypeIdentifier = o.CustomOption != null ? o.CustomOption.Identifier : null
                    });
        }

        public static IList<RegionModel> GetLayoutRegionsList(IRepository repository, Guid layoutId)
        {
            return GetLayoutRegionsQuery(repository, layoutId).OrderBy(lr => lr.RegionIdentifier).ToList();
        }

        public static DataListResponse<RegionModel> GetLayoutRegionsResponse(IRepository repository, Guid layoutId, RequestBase<DataOptions> request)
        {
            request.Data.SetDefaultOrder("RegionIdentifier");

            return GetLayoutRegionsQuery(repository, layoutId).ToDataListResponse(request);
        }

        private static IQueryable<RegionModel> GetLayoutRegionsQuery(IRepository repository, Guid layoutId)
        {
            return repository
                .AsQueryable<LayoutRegion>(lr => lr.Layout.Id == layoutId && !lr.Layout.IsDeleted && lr.Region != null && !lr.Region.IsDeleted)
                .Select(lr => new RegionModel
                    {
                        Id = lr.Region.Id,
                        Version = lr.Region.Version,
                        CreatedBy = lr.CreatedByUser,
                        CreatedOn = lr.CreatedOn,
                        LastModifiedBy = lr.ModifiedByUser,
                        LastModifiedOn = lr.ModifiedOn,

                        RegionIdentifier = lr.Region.RegionIdentifier,
                        Description = lr.Description
                    });
        }
    }
}