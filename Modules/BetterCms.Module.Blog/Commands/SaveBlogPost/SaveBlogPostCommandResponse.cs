// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveBlogPostCommandResponse.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.ViewModels.Content;

namespace BetterCms.Module.Blog.Commands.SaveBlogPost
{
    public class SaveBlogPostCommandResponse
    {
        /// <summary>
        /// Gets or sets the blog id.
        /// </summary>
        /// <value>
        /// The blog id.
        /// </value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content identifier.
        /// </summary>
        /// <value>
        /// The content identifier.
        /// </value>
        public virtual Guid ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page content id.
        /// </summary>
        /// <value>
        /// The page content id.
        /// </value>
        public virtual Guid PageContentId { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the content version.
        /// </summary>
        /// <value>
        /// The content version.
        /// </value>
        public virtual int ContentVersion { get; set; }

        /// <summary>
        /// Gets or sets the page URL.
        /// </summary>
        /// <value>
        /// The page URL.
        /// </value>
        public virtual string PageUrl { get; set; }

        /// <summary>
        /// Gets or sets the blog title.
        /// </summary>
        /// <value>
        /// The blog title.
        /// </value>
        public virtual string Title { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is modified on.
        /// </summary>
        /// <value>
        /// The date the blog post is modified on.
        /// </value>
        public string ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the date the blog post is created on.
        /// </summary>
        /// <value>
        /// The date the blog post is created on.
        /// </value>
        public string CreatedOn { get; set; }

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
        /// Gets or sets the desirable status.
        /// </summary>
        /// <value>
        /// The desirable status.
        /// </value>
        public ContentStatus DesirableStatus { get; set; }

        /// <summary>
        /// Gets or sets the regions.
        /// </summary>
        /// <value>
        /// The regions.
        /// </value>
        public List<PageContentChildRegionViewModel> Regions { get; set; }
    }
}