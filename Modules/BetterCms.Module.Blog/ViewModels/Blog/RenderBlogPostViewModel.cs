// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderBlogPostViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Author;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class RenderBlogPostViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderBlogPostViewModel" /> class.
        /// </summary>
        /// <param name="blogPost">The blog post.</param>
        /// <param name="content">The content.</param>
        public RenderBlogPostViewModel(BlogPost blogPost = null, BlogPostContent content = null)
        {
            if (content != null)
            {
                ActivationDate = content.ActivationDate;
                ExpirationDate = content.ExpirationDate;
            }

            if (blogPost != null)
            {
                if (blogPost.Author != null)
                {
                    Author = new RenderBlogPostAuthorViewModel(blogPost.Author);
                }

                if (content == null)
                {
                    ActivationDate = blogPost.ActivationDate;
                    ExpirationDate = blogPost.ExpirationDate;
                }
            }
        }

        /// <summary>
        /// Gets or sets the blog post author.
        /// </summary>
        /// <value>
        /// The blog post author.
        /// </value>
        public RenderBlogPostAuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the blog post activation date.
        /// </summary>
        /// <value>
        /// The blog post activation date.
        /// </value>
        public DateTime ActivationDate { get; set; }

        /// <summary>
        /// Gets or sets the blog post expiration date.
        /// </summary>
        /// <value>
        /// The blog post expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, ActivationDate: {1}, ExpirationDate: {2}", base.ToString(), ActivationDate, ExpirationDate);
        }
    }
}