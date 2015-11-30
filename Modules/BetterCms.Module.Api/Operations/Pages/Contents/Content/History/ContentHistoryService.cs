// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentHistoryService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

using ServiceStack.ServiceInterface;

using CoreContentStatus = BetterCms.Core.DataContracts.Enums.ContentStatus;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    public class ContentHistoryService : Service, IContentHistoryService
    {
        private readonly IHistoryService historyService;

        public ContentHistoryService(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        public GetContentHistoryResponse Get(GetContentHistoryRequest request)
        {
            var results = historyService.GetContentHistory(request.ContentId, new SearchableGridOptions())
                .AsQueryable()
                .OrderBy(history => history.CreatedOn)
                .Select(history => new
                    {
                        Type = history.GetType(),
                        Model = new HistoryContentModel
                            {
                                Id = history.Id,
                                Version = history.Version,
                                CreatedBy = history.CreatedByUser,
                                CreatedOn = history.CreatedOn,
                                LastModifiedBy = history.ModifiedByUser,
                                LastModifiedOn = history.ModifiedOn,

                                OriginalContentId = history.Original != null ? history.Original.Id : (System.Guid?)null,
                                PublishedOn = history.Status == CoreContentStatus.Published ? history.PublishedOn : null,
                                PublishedByUser = history.Status == CoreContentStatus.Published ? history.PublishedByUser : null,
                                ArchivedOn = history.Status == CoreContentStatus.Archived ? history.CreatedOn : (System.DateTime?)null,
                                ArchivedByUser = history.Status == CoreContentStatus.Archived ? history.CreatedByUser : null,
                                Status = (ContentStatus)((int)history.Status)
                            }
                    })
                .ToList();

            // Set content types
            results.ForEach(item => item.Model.ContentType = item.Type.ToContentTypeString());

            return new GetContentHistoryResponse
                       {
                           Data = results.Select(item => item.Model).ToList()
                       };
        }
    }
}