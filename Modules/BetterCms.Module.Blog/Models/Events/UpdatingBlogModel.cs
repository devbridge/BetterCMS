// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UpdatingBlogModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Models.Events
{
    public class UpdatingBlogModel
    {
        public UpdatingBlogModel(PageProperties pageProperties)
        {
            this.PageUrl = pageProperties.PageUrl;
            this.PageUrlHash = pageProperties.PageUrlHash;
            this.Title = pageProperties.Title;
            this.Description = pageProperties.Description;
            this.CustomCss = pageProperties.CustomCss;
            this.CustomJS = pageProperties.CustomJS;
            this.MetaTitle = pageProperties.MetaTitle;
            this.MetaKeywords = pageProperties.MetaKeywords;
            this.MetaDescription = pageProperties.MetaDescription;

            this.Status = pageProperties.Status;
            this.PublishedOn = pageProperties.PublishedOn;

            this.HasSEO = pageProperties.HasSEO;
            this.UseCanonicalUrl = pageProperties.UseCanonicalUrl;
            this.UseNoFollow = pageProperties.UseNoFollow;
            this.UseNoIndex = pageProperties.UseNoIndex;
            this.IsMasterPage = pageProperties.IsMasterPage;
            this.IsArchived = pageProperties.IsArchived;

            this.IsInSitemap = pageProperties.IsInSitemap;

            this.LayoutId = pageProperties.Layout != null ? pageProperties.Layout.Id : (Guid?)null;
            this.MasterPageId = pageProperties.MasterPage != null ? pageProperties.MasterPage.Id : (Guid?)null;
            Categories = pageProperties.Categories != null ? pageProperties.Categories.Select(c => c.Id).ToList() : new List<Guid>();
            this.MainImageId = pageProperties.Image != null ? pageProperties.Image.Id : (Guid?)null;
            this.SecondaryImageId = pageProperties.SecondaryImage != null ? pageProperties.SecondaryImage.Id : (Guid?)null;
            this.FeaturedImageId = pageProperties.FeaturedImage != null ? pageProperties.FeaturedImage.Id : (Guid?)null;
        }

        public string PageUrl { get; private set; }
        public string PageUrlHash { get; private set; }
        public string Title { get; private set; }
        public string Description { get; set; }
        public string CustomCss { get; set; }
        public string CustomJS { get; set; }
        public string MetaTitle { get; private set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public PageStatus Status { get; private set; }
        public DateTime? PublishedOn { get; private set; }

        public bool HasSEO { get; private set; }
        public bool UseCanonicalUrl { get; private set; }
        public bool UseNoFollow { get; private set; }
        public bool UseNoIndex { get; private set; }
        public bool IsMasterPage { get; private set; }
        public bool IsArchived { get; private set; }

        public bool IsInSitemap { get; private set; }

        public Guid? LayoutId { get; private set; }
        public Guid? MasterPageId { get; private set; }
        public IList<Guid> Categories { get; private set; }
        public Guid? MainImageId { get; private set; }
        public Guid? SecondaryImageId { get; private set; }
        public Guid? FeaturedImageId { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "{0}, Title: {1}, Url: {2}, Status: {3}, HasSEO: {4}, LayoutId: {5}, MasterPageId: {6}",
                base.ToString(),
                this.Title,
                this.PageUrl,
                this.Status,
                this.HasSEO,
                this.LayoutId,
                this.MasterPageId);
        }
    }
}