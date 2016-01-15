// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategorizableItemsService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Root.Models;

using BetterModules.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.CategorizableItems
{
    public class CategorizableItemsService : Service, ICategorizableItemsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        public CategorizableItemsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetCategorizableItemsResponse Get(GetCategorizableItemsRequest request)
        {
            request.Data.SetDefaultOrder("Name");
            var response = repository
                .AsQueryable<CategorizableItem>()
                .Select(ci => new CategorizableItemModel
                {
                    Id = ci.Id,
                    Version = ci.Version,
                    CreatedBy = ci.CreatedByUser,
                    CreatedOn = ci.CreatedOn,
                    LastModifiedBy = ci.ModifiedByUser,
                    LastModifiedOn = ci.ModifiedOn,

                    Name = ci.Name
                })
                .ToDataListResponse(request);

            return new GetCategorizableItemsResponse
            {
                Data = response
            };
        }
    }
}