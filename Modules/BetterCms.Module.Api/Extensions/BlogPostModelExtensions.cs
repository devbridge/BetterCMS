// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogPostModelExtensions.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.Security;

using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostModelExtensions
    {
        public static BlogPostViewModelExtender ToServiceModel(this SaveBlogPostPropertiesModel model)
        {
            var serviceModel = new BlogPostViewModelExtender();
            
            serviceModel.Version = model.Version;
            serviceModel.Title = model.Title;
            serviceModel.IntroText = model.IntroText;
            serviceModel.Content = model.HtmlContent;
            serviceModel.OriginalText = model.OriginalText;
            serviceModel.ContentTextMode = (ContentTextMode)model.ContentTextMode;
            serviceModel.LiveFromDate = model.ActivationDate;
            serviceModel.LiveToDate = model.ExpirationDate;
            serviceModel.BlogUrl = model.BlogPostUrl;
            serviceModel.AuthorId = model.AuthorId;
            serviceModel.Categories = model.Categories != null ? model.Categories.Select(c => new LookupKeyValue()
            {
                Key = c.ToString(),
            }).ToList() : new List<LookupKeyValue>();
            serviceModel.DesirableStatus = model.IsPublished ? ContentStatus.Published : ContentStatus.Draft;
            serviceModel.Tags = model.Tags;
            serviceModel.Image = new ImageSelectorViewModel { ImageId = model.MainImageId };
            serviceModel.FeaturedImageId = model.FeaturedImageId;
            serviceModel.SecondaryImageId = model.SecondaryImageId;
            serviceModel.IsArchived = model.IsArchived;
            serviceModel.PublishedOn = model.PublishedOn;
            serviceModel.LayoutId = model.LayoutId;
            serviceModel.MasterPageId = model.MasterPageId;

            serviceModel.ContentId = model.TechnicalInfo != null && model.TechnicalInfo.BlogPostContentId.HasValue ? model.TechnicalInfo.BlogPostContentId.Value : Guid.Empty;
            serviceModel.PageContentId = model.TechnicalInfo != null && model.TechnicalInfo.PageContentId.HasValue ? model.TechnicalInfo.PageContentId.Value : (Guid?)null;
            serviceModel.RegionId = model.TechnicalInfo != null && model.TechnicalInfo.RegionId.HasValue ? model.TechnicalInfo.RegionId.Value : (Guid?)null;

            serviceModel.UseCanonicalUrl = model.UseCanonicalUrl;
            serviceModel.UseNoFollow = model.UseNoFollow;
            serviceModel.UseNoIndex = model.UseNoIndex;
            serviceModel.MetaKeywords = model.MetaData != null ? model.MetaData.MetaKeywords : null;
            serviceModel.MetaDescription = model.MetaData != null ? model.MetaData.MetaDescription : null;
            serviceModel.MetaTitle = model.MetaData != null ? model.MetaData.MetaTitle : null;

            if (model.Language != null)
            {
                serviceModel.UpdateLanguage = true;
                serviceModel.LanguageGroupIdentifier = model.Language.LanguageGroupIdentifier;
                serviceModel.LanguageId = model.Language.Id;
            }

            if (model.AccessRules != null)
            {
                serviceModel.AccessRules = model.AccessRules
                    .Select(r => (IAccessRule)new UserAccessViewModel
                            {
                                AccessLevel = (AccessLevel)(int)r.AccessLevel, 
                                Identity = r.Identity, 
                                IsForRole = r.IsForRole
                            })
                    .ToList();
            }

            return serviceModel;
        }
    }
}