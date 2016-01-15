// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageExtensions.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;

namespace BetterCms.Module.Api.Extensions
{
    public static class PageExtensions
    {
        public static PutPagePropertiesRequest ToPutRequest(this GetPagePropertiesResponse response)
        {
            var model = MapPageModel(response);

            return new PutPagePropertiesRequest { Data = model, Id = response.Data.Id };
        }

        public static PostPagePropertiesRequest ToPostRequest(this GetPagePropertiesResponse response)
        {
            var model = MapPageModel(response);

            return new PostPagePropertiesRequest { Data = model };
        }

        private static SavePagePropertiesModel MapPageModel(GetPagePropertiesResponse response)
        {
            var model = new SavePagePropertiesModel
            {
                Version = response.Data.Version,
                PageUrl = response.Data.PageUrl,
                Title = response.Data.Title,
                Description = response.Data.Description,
                IsPublished = response.Data.IsPublished,
                PublishedOn = response.Data.PublishedOn,
                LayoutId = response.Data.LayoutId,
                MasterPageId = response.Data.MasterPageId,
                Categories = response.Data.Categories,
                IsArchived = response.Data.IsArchived,
                MainImageId = response.Data.MainImageId,
                FeaturedImageId = response.Data.FeaturedImageId,
                SecondaryImageId = response.Data.SecondaryImageId,
                CustomCss = response.Data.CustomCss,
                CustomJavaScript = response.Data.CustomJavaScript,
                UseCanonicalUrl = response.Data.UseCanonicalUrl,
                UseNoFollow = response.Data.UseNoFollow,
                UseNoIndex = response.Data.UseNoIndex,
                IsMasterPage = response.Data.IsMasterPage,
                LanguageId = response.Data.LanguageId,
                LanguageGroupIdentifier = response.Data.LanguageGroupIdentifier,
                ForceAccessProtocol = response.Data.ForceAccessProtocol,
                MetaData = response.MetaData,
                AccessRules = response.AccessRules,
                PageOptions = response.PageOptions,
            };

            if (response.Tags != null)
            {
                model.Tags = response.Tags.Select(t => t.Name).ToList();
            }

            return model;
        }
    }
}
