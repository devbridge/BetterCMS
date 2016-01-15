// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentsService.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    public class PageContentsService : Service, IPageContentsService
    {
        private readonly IRepository repository;

        public PageContentsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetPageContentsResponse Get(GetPageContentsRequest request)
        {
            request.Data.SetDefaultOrder("Order");

            var query = repository
                 .AsQueryable<Module.Root.Models.PageContent>(pageContent => pageContent.Page.Id == request.PageId && !pageContent.Page.IsDeleted && !pageContent.Content.IsDeleted);

            if (request.Data.RegionId.HasValue)
            {
                query = query.Where(pageContent => pageContent.Region != null && !pageContent.Region.IsDeleted && pageContent.Region.Id == request.Data.RegionId);
            }
            else if (!string.IsNullOrWhiteSpace(request.Data.RegionIdentifier))
            {
                query = query.Where(pageContent => pageContent.Region != null && !pageContent.Region.IsDeleted && pageContent.Region.RegionIdentifier == request.Data.RegionIdentifier);
            }

            if (!request.Data.IncludeUnpublished)
            {
                query = query.Where(pageContent => pageContent.Content.Status == ContentStatus.Published);
            }

            var dataListResult = query.Select(pageContent => new PageContentModel
                     {
                         Id = pageContent.Id,
                         Version = pageContent.Version,
                         CreatedBy = pageContent.CreatedByUser,
                         CreatedOn = pageContent.CreatedOn,
                         LastModifiedBy = pageContent.ModifiedByUser,
                         LastModifiedOn = pageContent.ModifiedOn,

                         ContentId = pageContent.Content.Id,
                         ParentPageContentId = pageContent.Parent != null ? pageContent.Parent.Id : (System.Guid?)null,
                         OriginalContentType = pageContent.Content.GetType(),
                         Name = pageContent.Content.Name,
                         RegionId = pageContent.Region.Id,
                         RegionIdentifier = pageContent.Region.RegionIdentifier,
                         Order = pageContent.Order,
                         IsPublished = pageContent.Content.Status == ContentStatus.Published
                     }).ToDataListResponse(request);

            // Set content types
            dataListResult.Items.ToList().ForEach(item => item.ContentType = item.OriginalContentType.ToContentTypeString());

            return new GetPageContentsResponse { Data = dataListResult };
        }
    }
}