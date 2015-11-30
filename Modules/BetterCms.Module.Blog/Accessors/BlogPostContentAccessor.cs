// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogPostContentAccessor.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.ViewModels.History;

namespace BetterCms.Module.Blog.Accessors
{
    [Serializable]
    public class BlogPostContentAccessor : ContentAccessor<BlogPostContent>
    {
        public const string ContentWrapperType = "blog-post-content";

        public BlogPostContentAccessor(BlogPostContent content, IList<IOptionValue> options)
            : base(content, options)
        {
        }

        public override string GetContentWrapperType()
        {
            return ContentWrapperType;
        }

        public override string GetHtml(HtmlHelper html)
        {
            if (!string.IsNullOrWhiteSpace(Content.Html))
            {
                return Content.Html;
            }

            return "&nbsp;";
        }

        public override string[] GetCustomStyles(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetCustomJavaScript(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetStylesResources(HtmlHelper html)
        {
            return null;
        }

        public override string[] GetJavaScriptResources(HtmlHelper html)
        {
            return null;
        }

        public override PropertiesPreview GetHtmlPropertiesPreview()
        {
            return new PropertiesPreview
            {
                ViewName = "~/Areas/bcms-blog/Views/History/BlogPropertiesHistory.cshtml",
                ViewModel = new HtmlContentHistoryViewModel
                {
                    Name = Content.Name,
                    ActivationDate = Content.ActivationDate,
                    ExpirationDate = Content.ExpirationDate,
                    RowText = Content.ContentTextMode == ContentTextMode.Html ? Content.Html : Content.OriginalText,
                    UseCustomCss = Content.UseCustomCss,
                    CustomCss = Content.CustomCss,
                    UseCustomJs = Content.UseCustomJs,
                    CustomJs = Content.CustomJs,
                }
            };
        }
    }
}