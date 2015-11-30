// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogPostExtensions.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Api.Operations.Blog.Authors.Author;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.BlogPost.Properties;
using BetterCms.Module.Api.Operations.Blog.BlogPosts.Settings;
using BetterCms.Module.Api.Operations.Pages;

namespace BetterCms.Module.Api.Extensions
{
    public static class BlogPostExtensions
    {
        public static PutBlogPostPropertiesRequest ToPutRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PutBlogPostPropertiesRequest { Data = model, Id = response.Data.Id };
        }
        
        public static PostBlogPostPropertiesRequest ToPostRequest(this GetBlogPostPropertiesResponse response)
        {
            var model = MapBlogPostModel(response);

            return new PostBlogPostPropertiesRequest { Data = model };
        }

        public static PutAuthorRequest ToPutRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PutAuthorRequest { Data = model, Id = response.Data.Id };
        }

        public static PostAuthorRequest ToPostRequest(this GetAuthorResponse response)
        {
            var model = MapAuthorModel(response);

            return new PostAuthorRequest { Data = model };
        }

        public static PutBlogPostsSettingsRequest ToPutRequest(this GetBlogPostsSettingsResponse response)
        {
            var model = MapSettingsModel(response);

            return new PutBlogPostsSettingsRequest { Data = model };
        }

        private static SaveBlogPostsSettingsModel MapSettingsModel(GetBlogPostsSettingsResponse response)
        {
            return new SaveBlogPostsSettingsModel
                       {
                           Version = response.Data != null ? response.Data.Version : 0,
                           DefaultMasterPageId = response.Data != null ? response.Data.DefaultMasterPageId : null,
                           DefaultLayoutId = response.Data != null ? response.Data.DefaultLayoutId : null
                       };
        }

        private static SaveBlogPostPropertiesModel MapBlogPostModel(GetBlogPostPropertiesResponse response)
        {
            var model = new SaveBlogPostPropertiesModel
                        {
                            Version = response.Data.Version,
                            BlogPostUrl = response.Data.BlogPostUrl,
                            Title = response.Data.Title,
                            IntroText = response.Data.IntroText,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            LayoutId = response.Data.LayoutId,
                            MasterPageId = response.Data.MasterPageId,
                            Categories = response.Data.Categories,
                            AuthorId = response.Data.AuthorId,
                            MainImageId = response.Data.MainImageId,
                            FeaturedImageId = response.Data.FeaturedImageId,
                            SecondaryImageId = response.Data.SecondaryImageId,
                            ActivationDate = response.Data.ActivationDate,
                            ExpirationDate = response.Data.ExpirationDate,
                            IsArchived = response.Data.IsArchived,
                            UseCanonicalUrl = response.Data.UseCanonicalUrl,
                            UseNoFollow = response.Data.UseNoFollow,
                            UseNoIndex = response.Data.UseNoIndex,
                            HtmlContent = response.HtmlContent,
                            OriginalText = response.OriginalText,
                            ContentTextMode = response.ContentTextMode ?? ContentTextMode.Html,
                            MetaData = response.MetaData,
                            Language = response.Language,
                            TechnicalInfo = response.TechnicalInfo,
                            AccessRules = response.AccessRules,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            if (response.Tags != null)
            {
                model.Tags = response.Tags.Select(t => t.Name).ToList();
            }

            return model;
        }

        private static SaveAuthorModel MapAuthorModel(GetAuthorResponse response)
        {
            var model = new SaveAuthorModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            Description = response.Data.Description,
                            ImageId = response.Data.ImageId
                        };

            return model;
        }
    }
}
