// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorsService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.MediaManager.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.Authors
{
    public class AuthorsService : Service, IAuthorsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The file URL resolver.
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// The author service.
        /// </summary>
        private readonly IAuthorService authorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorsService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        /// <param name="authorService">The author service.</param>
        public AuthorsService(IRepository repository, IMediaFileUrlResolver fileUrlResolver, IAuthorService authorService)
        {
            this.repository = repository;
            this.fileUrlResolver = fileUrlResolver;
            this.authorService = authorService;
        }

        /// <summary>
        /// Gets authors list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>GetAuthorsResponse</c> with authors list.
        /// </returns>
        public GetAuthorsResponse Get(GetAuthorsRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Blog.Models.Author>()
                .Select(author => new AuthorModel
                    {
                        Id = author.Id,
                        Version = author.Version,
                        CreatedBy = author.CreatedByUser,
                        CreatedOn = author.CreatedOn,
                        LastModifiedBy = author.ModifiedByUser,
                        LastModifiedOn = author.ModifiedOn,

                        Name = author.Name,
                        Description = author.Description,

                        ImageId = author.Image != null && !author.Image.IsDeleted ? author.Image.Id : (Guid?)null,
                        ImageUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicUrl : (string)null,
                        ImageThumbnailUrl = author.Image != null && !author.Image.IsDeleted ? author.Image.PublicThumbnailUrl : (string)null,
                        ImageCaption = author.Image != null && !author.Image.IsDeleted ? author.Image.Caption : (string)null
                    })
                .ToDataListResponse(request);

            foreach (var model in listResponse.Items)
            {
                model.ImageUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageUrl);
                model.ImageThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(model.ImageThumbnailUrl);
            }

            return new GetAuthorsResponse
                       {
                           Data = listResponse
                       };
        }

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        ///   <c>PostAuthorsResponse</c> with a new author id.
        /// </returns>
        public PostAuthorResponse Post(PostAuthorRequest request)
        {
            var result =
                authorService.Put(
                    new PutAuthorRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostAuthorResponse { Data = result.Data };
        }
    }
}