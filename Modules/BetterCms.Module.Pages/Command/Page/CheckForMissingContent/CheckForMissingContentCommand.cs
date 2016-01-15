// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CheckForMissingContentCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.CheckForMissingContent
{
    public class CheckForMissingContentCommand : CommandBase, ICommand<CheckForMissingContentRequest, CheckForMissingContentResponse>
    {
        public CheckForMissingContentResponse Execute(CheckForMissingContentRequest request)
        {
            var page = Repository.AsQueryable<PageProperties>(l => l.Id == request.PageId)
                .FetchMany(x => x.PageContents)
                .ThenFetch(x => x.Region)
                .ToList()
                .FirstOne();

            var pageRegions = page.PageContents != null ? page.PageContents.Select(x => x.Region).ToList() : new List<Region>();
            
            var layoutRegions = new List<Region>();

            if (request.MasterPageId.HasValue && request.MasterPageId != Guid.Empty)
            {
                var masterPage = Repository.AsQueryable<Root.Models.Page>(mp => mp.Id == request.MasterPageId)
                    .FetchMany(mp => mp.PageContents)
                    .ThenFetch(pc => pc.Content)
                    .ToList()
                    .FirstOne();

                var contents = masterPage.PageContents.Select(x => x.Content);
                foreach (var content in contents)
                {
                    var regions = content.ContentRegions.Select(x => x.Region);
                    layoutRegions.AddRange(regions);
                }

                layoutRegions = layoutRegions.Distinct().ToList();
            }
            else if (request.TemplateId.HasValue && request.TemplateId != Guid.Empty)
            {
                var layout = Repository.AsQueryable<Root.Models.Layout>(l => l.Id == request.TemplateId)
                    .FetchMany(l => l.LayoutRegions)
                    .ThenFetch(lr => lr.Region)
                    .ToList()
                    .FirstOne();

                layoutRegions = layout.LayoutRegions != null ? layout.LayoutRegions.Select(x => x.Region).ToList() : new List<Region>(); 
            }

            var response = new CheckForMissingContentResponse { IsMissingContents = false };

            var missingRegions = pageRegions.Where(pr => layoutRegions.All(lr => lr.RegionIdentifier != pr.RegionIdentifier)).ToList();

            if (missingRegions.Any())
            {
                response.IsMissingContents = true;
            }

            return response;
        }

    }
}