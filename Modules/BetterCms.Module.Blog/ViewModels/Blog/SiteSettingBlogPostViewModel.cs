// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SiteSettingBlogPostViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Blog.ViewModels.Blog
{
    public class SiteSettingBlogPostViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value>
        /// The blog id.
        /// </value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is created on.
        /// </summary>
        /// <value>
        /// The date the blog post is created on.
        /// </value>
        public string CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is modified on.
        /// </summary>
        /// <value>
        /// The date the blog post is modified on.
        /// </value>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the user, name which last modified the blog post.
        /// </summary>
        /// <value>
        /// The user name, which last modified the blog post.
        /// </value>
        public string ModifiedByUser { get; set; }

        /// <summary>
        /// Gets or sets the page status.
        /// </summary>
        /// <value>
        /// The page status.
        /// </value>
        public PageStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the page url.
        /// </summary>
        /// <value>
        /// The page url.
        /// </value>
        public string PageUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Title: {2}", Id, Version, Title);
        }
    }
}