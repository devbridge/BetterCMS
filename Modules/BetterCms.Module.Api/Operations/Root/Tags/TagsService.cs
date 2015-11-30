// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagsService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Tags.Tag;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Tags
{

    /// <summary>
    /// Default tags service contract implementation for REST.
    /// </summary>
    public class TagsService : Service, ITagsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The tag service.
        /// </summary>
        private readonly ITagService tagService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="tagService">The tag service.</param>
        public TagsService(IRepository repository, ITagService tagService)
        {
            this.repository = repository;
            this.tagService = tagService;
        }

        /// <summary>
        /// Gets the tags list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetTagsResponse</c> with tags list.
        /// </returns>
        public GetTagsResponse Get(GetTagsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Tag>()
                .Select(tag => new TagModel
                    {
                        Id = tag.Id,
                        Version = tag.Version,
                        CreatedBy = tag.CreatedByUser,
                        CreatedOn = tag.CreatedOn,
                        LastModifiedBy = tag.ModifiedByUser,
                        LastModifiedOn = tag.ModifiedOn,

                        Name = tag.Name
                    })
                .ToDataListResponse(request);

            return new GetTagsResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostTagsResponse</c> with a new tag id.
        /// </returns>
        public PostTagResponse Post(PostTagRequest request)
        {
            var result =
                tagService.Put(
                    new PutTagRequest
                        {
                            Data = request.Data,
                            User = request.User
                        });

            return new PostTagResponse { Data = result.Data };
        }
    }
}