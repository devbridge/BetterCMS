// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlContentWidgetAccessor.cs" company="Devbridge Group LLC">
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
using System.Text;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Mvc.PageHtmlRenderer;

namespace BetterCms.Module.Pages.Accessors
{
    [Serializable]
    public class HtmlContentWidgetAccessor : ContentAccessor<HtmlContentWidget>
    {
        public const string ContentWrapperType = "html-widget";

        public HtmlContentWidgetAccessor(HtmlContentWidget content, IList<IOptionValue> options)
            : base(content, options)
        {
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper html)
        {
            if (Content.UseHtml && !string.IsNullOrWhiteSpace(Content.Html))
            {
                var builder = new StringBuilder(Content.Html);
                builder = PageContentHtmlRenderer.GetReplacedHtml(builder, Options);

                return builder.ToString();
            }

            return "&nbsp;";
        }

        public override string[] GetCustomStyles(HtmlHelper html)
        {
            if (Content.UseCustomCss && !string.IsNullOrWhiteSpace(Content.CustomCss))
            {
                var css = CssHelper.FixCss(Content.CustomCss);
                if (!string.IsNullOrWhiteSpace(css))
                {
                    return new[] { css };
                }
            }

            return null;
        }

        public override string[] GetCustomJavaScript(HtmlHelper html)
        {
            if (Content.UseCustomJs && !string.IsNullOrWhiteSpace(Content.CustomJs))
            {
                return new[] { Content.CustomJs };
            }

            return null;
        }

        public override string[] GetStylesResources(HtmlHelper html)
        {
            if (Options != null)
            {
                return Options.ToStyleResources();
            }

            return null;
        }

        public override string[] GetJavaScriptResources(HtmlHelper html)
        {
            if (Options != null)
            {
                return Options.ToJavaScriptResources();
            }

            return null;
        }

        public override PropertiesPreview GetHtmlPropertiesPreview()
        {
            return new PropertiesPreview
            {
                ViewName = "~/Areas/bcms-pages/Views/History/WidgetPropertiesHistory.cshtml",
                ViewModel = new HtmlContentWidgetHistoryViewModel
                {
                    Name = Content.Name,
                    Html = Content.Html,
                    UseCustomHtml = Content.UseHtml,
                    UseCustomCss = Content.UseCustomCss,
                    CustomCss = Content.CustomCss,
                    UseCustomJs = Content.UseCustomJs,
                    CustomJs = Content.CustomJs,
                }
            };
        }
    }
}