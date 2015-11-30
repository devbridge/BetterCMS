// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBlogService.cs" company="Devbridge Group LLC">
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
using System.Security.Principal;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Blog.ViewModels.Filter;
using BetterCms.Module.Root.ViewModels.Option;

using NHibernate;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogService
    {
        /// <summary>
        /// Creates the blog URL from the given blog title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="unsavedUrls">The list of not saved yet urls.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>
        /// Created blog URL
        /// </returns>
        string CreateBlogPermalink(string title, List<string> unsavedUrls = null, IEnumerable<Guid> categoryId = null);

        /// <summary>
        /// Saves the blog post.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="childContentOptionValues">The child content option values.</param>
        /// <param name="principall">The principall.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <param name="updateActivationIfNotChanged">if set to <c>true</c> update activation time even if it was not changed.</param>
        /// <returns>
        /// Saved blog post entity
        /// </returns>
        BlogPost SaveBlogPost(BlogPostViewModel model, IList<ContentOptionValuesViewModel> childContentOptionValues, IPrincipal principall, out string[] errorMessages, bool updateActivationIfNotChanged = true);

        /// <summary>
        /// Gets the filtered blog posts query.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="joinContents">if set to <c>true</c> join contents tables.</param>
        /// <returns>
        /// NHibernate query for getting filtered blog posts
        /// </returns>
        IQueryOver<BlogPost, BlogPost> GetFilteredBlogPostsQuery(BlogsFilter filter, bool joinContents = false);
    }
}