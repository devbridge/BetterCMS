// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetBlogPostListCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Filter;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.Extensions;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetBlogPostList
{
    /// <summary>
    /// A command for get blogs list by filter.
    /// </summary>
    public class GetBlogPostListCommand : CommandBase, ICommand<BlogsFilter, BlogsGridViewModel<SiteSettingBlogPostViewModel>>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly ICmsConfiguration configuration;

        /// <summary>
        /// The languages service
        /// </summary>
        private readonly ILanguageService languageService;

        /// <summary>
        /// The blog service
        /// </summary>
        private readonly IBlogService blogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogPostListCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="blogService">The blog service.</param>
        public GetBlogPostListCommand(ICategoryService categoryService, ICmsConfiguration configuration,
            ILanguageService languageService, IBlogService blogService)
        {
            this.categoryService = categoryService;
            this.configuration = configuration;
            this.languageService = languageService;
            this.blogService = blogService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>A list of blog posts</returns>
        public BlogsGridViewModel<SiteSettingBlogPostViewModel> Execute(BlogsFilter request)
        {
            BlogPost alias = null;
//            SiteSettingBlogPostViewModel modelAlias = null;
            BlogPost modelAlias = null;

            var query = blogService.GetFilteredBlogPostsQuery(request);

            query = query
                .SelectList(select => select
                    .Select(() => alias.Id).WithAlias(() => modelAlias.Id)
                    .Select(() => alias.Title).WithAlias(() => modelAlias.Title)
                    .Select(() => alias.CreatedOn).WithAlias(() => modelAlias.CreatedOn)
                    .Select(() => alias.ModifiedOn).WithAlias(() => modelAlias.ModifiedOn)
                    .Select(() => alias.ModifiedByUser).WithAlias(() => modelAlias.ModifiedByUser)
                    .Select(() => alias.Status).WithAlias(() => modelAlias.Status)
                    .Select(() => alias.Version).WithAlias(() => modelAlias.Version)
                    .Select(() => alias.PageUrl).WithAlias(() => modelAlias.PageUrl))
                .TransformUsing(Transformers.AliasToBean<BlogPost>());

            var count = query.ToRowCountFutureValue();

            var blogPosts = query.AddSortingAndPaging(request).Future<BlogPost>();
            IEnumerable<LookupKeyValue> languagesFuture = configuration.EnableMultilanguage ? languageService.GetLanguagesLookupValues() : null;

            var categoriesLookupList = categoryService.GetCategoriesLookupList(BlogPost.CategorizableItemKeyForBlogs);

            var blogsList = new List<SiteSettingBlogPostViewModel>();
            foreach (var blogPost in blogPosts)
            {
                var blogModel = new SiteSettingBlogPostViewModel();
                blogModel.Id = blogPost.Id;
                blogModel.Title = blogPost.Title;
                blogModel.CreatedOn = blogPost.CreatedOn.ToFormattedDateString();
                blogModel.ModifiedOn = blogPost.ModifiedOn.ToFormattedDateString();
                blogModel.ModifiedByUser = blogPost.ModifiedByUser;
                blogModel.Status = blogPost.Status;
                blogModel.Version = blogPost.Version;
                blogModel.PageUrl = blogPost.PageUrl;
                blogsList.Add(blogModel);
            }
            var model = new BlogsGridViewModel<SiteSettingBlogPostViewModel>(blogsList, request, count.Value) { CategoriesLookupList = categoriesLookupList} ;
            if (languagesFuture != null)
            {
                model.Languages = languagesFuture.ToList();
                model.Languages.Insert(0, languageService.GetInvariantLanguageModel());
            }

            return model;
        }
    }
}