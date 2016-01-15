// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Nodes.Node;
using BetterCms.Module.Api.Operations.Pages.Sitemap.Tree;
using BetterCms.Module.Pages.Models;

using NHibernate.Linq;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Sitemap
{
    [Obsolete("Use everything from BetterCms.Module.Api.Operations.Pages.Sitemaps name space.")]
    public class SitemapService : Service, ISitemapService
    {
        private readonly IRepository repository;

        private readonly ISitemapTreeService treeService;

        private readonly INodesService nodesService;

        private readonly INodeService nodeService;

        public SitemapService(IRepository repository, ISitemapTreeService treeService, INodeService nodeService, INodesService nodesService)
        {
            this.repository = repository;
            this.treeService = treeService;
            this.nodeService = nodeService;
            this.nodesService = nodesService;
        }

        ISitemapTreeService ISitemapService.Tree
        {
            get
            {
                return treeService;
            }
        }

        INodesService ISitemapService.Nodes
        {
            get
            {
                return nodesService;
            }
        }

        INodeService ISitemapService.Node
        {
            get
            {
                return nodeService;
            }
        }

        public GetSitemapsResponse Get(GetSitemapsRequest request)
        {
            request.Data.SetDefaultOrder("Title");

            var query = repository
                .AsQueryable<Module.Pages.Models.Sitemap>();

            query = query.ApplySitemapTagsFilter(request.Data);

            var listResponse = query
                .Where(map => !map.IsDeleted)
                .Select(map => new SitemapModel
                {
                    Id = map.Id,
                    Version = map.Version,
                    CreatedBy = map.CreatedByUser,
                    CreatedOn = map.CreatedOn,
                    LastModifiedBy = map.ModifiedByUser,
                    LastModifiedOn = map.ModifiedOn,

                    Title = map.Title
                }).ToDataListResponse(request);

            if (listResponse.Items.Count > 0 && request.Data.IncludeTags)
            {
                LoadTags(listResponse, request.Data.IncludeTags);
            }

            return new GetSitemapsResponse
            {
                Data = listResponse
            };
        }

        private void LoadTags(DataListResponse<SitemapModel> response, bool includeTags)
        {
            var pageIds = response.Items.Select(i => i.Id).Distinct().ToArray();
            if (includeTags)
            {
                var tags = repository
                    .AsQueryable<SitemapTag>(pt => pageIds.Contains(pt.Sitemap.Id))
                    .Select(pt => new { PageId = pt.Sitemap.Id, TagName = pt.Tag.Name })
                    .OrderBy(o => o.TagName)
                    .ToFuture()
                    .ToList();

                response.Items.ToList().ForEach(page => { page.Tags = tags.Where(tag => tag.PageId == page.Id).Select(tag => tag.TagName).ToList(); });
            }
        }
    }
}