// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogPostsSettingsService.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings
{
    /// <summary>
    /// The blog posts settings service.
    /// </summary>
    public class BlogPostsSettingsService : Service, IBlogPostsSettingsService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogPostsSettingsService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public BlogPostsSettingsService(IRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>GetBlogPostsSettingsResponse</c> with settings.</returns>
        public GetBlogPostsSettingsResponse Get(GetBlogPostsSettingsRequest request)
        {
            return new GetBlogPostsSettingsResponse
                       {
                           Data =
                               repository.AsQueryable<Option>()
                               .Where(o => !o.IsDeleted)
                               .OrderByDescending(o => o.CreatedOn)
                               .Select(
                                   o =>
                                   new BlogPostsSettingsModel
                                       {
                                           Id = o.Id,
                                           Version = o.Version,
                                           CreatedOn = o.CreatedOn,
                                           CreatedBy = o.CreatedByUser,
                                           LastModifiedOn = o.ModifiedOn,
                                           LastModifiedBy = o.ModifiedByUser,
                                           DefaultLayoutId = o.DefaultLayout.Id,
                                           DefaultMasterPageId = o.DefaultMasterPage.Id
                                       })
                               .FirstOrDefault()
                       };
        }

        /// <summary>
        /// Puts the settings.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>PutBlogPostsSettingsResponse</c> with success status.</returns>
        public PutBlogPostsSettingsResponse Put(PutBlogPostsSettingsRequest request)
        {
            var option = repository.AsQueryable<Option>().OrderByDescending(o => o.CreatedOn).FirstOrDefault(o => !o.IsDeleted) ?? new Option();

            if (request.Data.DefaultMasterPageId.GetValueOrDefault() != default(Guid) && request.Data.DefaultLayoutId.GetValueOrDefault() != default(Guid))
            {
                throw new CmsApiValidationException("Only one of DefaultLayoutId and DefaultMasterPageId can have a value.");
            }

            if (request.Data.Version > 0)
            {
                option.Version = request.Data.Version;
            }

            if (request.Data.DefaultMasterPageId.GetValueOrDefault() != default(Guid))
            {
                option.DefaultMasterPage = repository.AsProxy<Page>(request.Data.DefaultMasterPageId.GetValueOrDefault());
                option.DefaultLayout = null;
            }
            else if (request.Data.DefaultLayoutId.GetValueOrDefault() != default(Guid))
            {
                option.DefaultLayout = repository.AsProxy<Layout>(request.Data.DefaultLayoutId.GetValueOrDefault());
                option.DefaultMasterPage = null;
            }
            else
            {
                option.DefaultMasterPage = null;
                option.DefaultLayout = null;
            }

            unitOfWork.BeginTransaction();
            repository.Save(option);
            unitOfWork.Commit();

            return new PutBlogPostsSettingsResponse { Data = true };
        }
    }
}