// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetExtensions.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget.ServerControlWidget;

namespace BetterCms.Module.Api.Extensions
{
    public static class WidgetExtensions
    {
        public static PutHtmlContentWidgetRequest ToPutRequest(this GetHtmlContentWidgetResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PutHtmlContentWidgetRequest { Data = model, Id = response.Data.Id };
        }

        public static PostHtmlContentWidgetRequest ToPostRequest(this GetHtmlContentWidgetResponse response)
        {
            var model = MapHtmlContentWidgetModel(response);

            return new PostHtmlContentWidgetRequest { Data = model };
        }

        private static SaveHtmlContentWidgetModel MapHtmlContentWidgetModel(GetHtmlContentWidgetResponse response)
        {
            var model = new SaveHtmlContentWidgetModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Categories = response.Categories.Select(c => c.Id).ToList(),
                            CustomCss = response.Data.CustomCss,
                            UseCustomCss = response.Data.UseCustomCss,
                            Html = response.Data.Html,
                            UseHtml = response.Data.UseHtml,
                            CustomJavaScript = response.Data.CustomJavaScript,
                            UseCustomJavaScript = response.Data.UseCustomJavaScript,
                            Options = response.Options,
                            ChildContentsOptionValues = response.ChildContentsOptionValues
                        };

            return model;
        }

        public static PutServerControlWidgetRequest ToPutRequest(this GetServerControlWidgetResponse response)
        {
            var model = MapServerControlWidgetModel(response);

            return new PutServerControlWidgetRequest { Data = model, Id = response.Data.Id };
        }

        public static PostServerControlWidgetRequest ToPostRequest(this GetServerControlWidgetResponse response)
        {
            var model = MapServerControlWidgetModel(response);

            return new PostServerControlWidgetRequest { Data = model };
        }

        private static SaveServerControlWidgetModel MapServerControlWidgetModel(GetServerControlWidgetResponse response)
        {
            var model = new SaveServerControlWidgetModel
                        {
                            Version = response.Data.Version,
                            Name = response.Data.Name,
                            IsPublished = response.Data.IsPublished,
                            PublishedOn = response.Data.PublishedOn,
                            PublishedByUser = response.Data.PublishedByUser,
                            Categories = response.Categories.Select(c => c.Id).ToList(),
                            PreviewUrl = response.Data.PreviewUrl,
                            WidgetUrl = response.Data.WidgetUrl,
                            Options = response.Options
                        };

            return model;
        }
    }
}
