// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RenderPagePropertiesViewModel.cs" company="Devbridge Group LLC">
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

using NHibernate;
using NHibernate.Linq;

namespace BetterCms.Module.Pages.ViewModels.Page
{
    public class RenderPagePropertiesViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RenderPagePropertiesViewModel" /> class.
        /// </summary>
        /// <param name="page">The page.</param>
        public RenderPagePropertiesViewModel(Models.PageProperties page = null)
        {
            Tags = new List<string>();
            Categories = new List<RenderPageCategoryViewModel>();
            if (page != null)
            {
                Description = page.Description;
                CustomCss = page.CustomCss;
                CustomJS = page.CustomJS;
                UseCanonicalUrl = page.UseCanonicalUrl;
                UseNoFollow = page.UseNoFollow;
                UseNoIndex = page.UseNoIndex;
                IsInSitemap = page.IsInSitemap;
                MetaKeywords = page.MetaKeywords;
                MetaDescription = page.MetaDescription;
                IsArchived = page.IsArchived;
                
                if (page.Image != null)
                {
                    MainImage = new RenderPageImageViewModel(page.Image);
                }
                if (page.FeaturedImage != null)
                {
                    FeaturedImage = new RenderPageImageViewModel(page.FeaturedImage);
                }
                if (page.SecondaryImage != null)
                {
                    SecondaryImage = new RenderPageImageViewModel(page.SecondaryImage);
                }
                if (page.Categories != null)
                {
                    foreach (var category in page.Categories)
                    {
                        Categories.Add(new RenderPageCategoryViewModel(category));
                    }                    
                }
                if (page.PageTags != null)
                {
                    foreach (var tag in page.PageTags.Distinct())
                    {
                        var tagName = tag.Tag.Name;
                        if (!Tags.Contains(tagName))
                        {
                            Tags.Add(tagName);
                        }
                    }
                }
            }
        }

        public string Description { get; set; }

        public string CustomCss { get; set; }

        public string CustomJS { get; set; }

        public bool UseCanonicalUrl { get; set; }

        public bool UseNoFollow { get; set; }

        public bool UseNoIndex { get; set; }

        public bool IsInSitemap { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public bool IsArchived { get; set; }

        public RenderPageImageViewModel MainImage { get; set; }

        public RenderPageImageViewModel FeaturedImage { get; set; }

        public RenderPageImageViewModel SecondaryImage { get; set; }

        public IList<RenderPageCategoryViewModel> Categories { get; set; }

        public IList<string> Tags { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Description: {1}", base.ToString(), Description);
        }
    }
}