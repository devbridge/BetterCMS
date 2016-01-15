// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentExtensions.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;

namespace BetterCms.Module.Api.Extensions
{
    public static class ContentExtensions
    {
        public static PutHtmlContentRequest ToPutRequest(this GetHtmlContentResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentRequest { Data = model, Id = response.Data.Id };
        }

        public static PutPageContentRequest ToPutRequest(this GetPageContentResponse response)
        {
            var model = MapPageContentWidgetModel(response);

            return new PutPageContentRequest { Data = model, PageId = response.Data.PageId, Id = response.Data.Id };
        }

        private static SaveHtmlContentModel MapHtmlContentWidgetModel(GetHtmlContentResponse response)
        {
            var model = new SaveHtmlContentModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            ActivationDate = response.Data.ActivationDate,
                            ExpirationDate = response.Data.ExpirationDate,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Html = response.Data.Html,
                            OriginalText = response.Data.OriginalText,
                            ContentTextMode = response.Data.ContentTextMode,
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            return model;
        }

        private static SavePageContentModel MapPageContentWidgetModel(GetPageContentResponse response)
        {
            var model = new SavePageContentModel
                {
                    Version = response.Data.Version,
                    ContentId = response.Data.ContentId,
                    ParentPageContentId = response.Data.ParentPageContentId,
                    RegionId = response.Data.RegionId,
                    Order = response.Data.Order,
                    Options = response.Options
                };

            return model;
        }
    }
}
