// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreviewHelper.cs" company="Devbridge Group LLC">
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
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    public static class PreviewHelper
    {
        public static IHtmlString PreviewContentBox(this HtmlHelper html, string previewUrl, string openUrl, string title = "Content Preview", bool asImage = false)
        {
            return PreviewBox(html, previewUrl, openUrl, title, "100%", "100%", "bcms-content-frame", asImage);
        }

        public static IHtmlString PreviewLayoutBox(this HtmlHelper html, string url, string title = "Layout Preview")
        {
            return PreviewBox(html, url, url, title, "930", "930", "bcms-layout-frame", false);
        }

        private static IHtmlString PreviewBox(HtmlHelper html, string previewUrl, string openUrl, string title, string width, string height, string frameCssClass, bool asImage)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(
                "<div class=\"bcms-grid-image-holder bcms-preview-zoom-box bcms-js-preview-box\" data-as-image=\"{0}\", data-preview-url=\"{1}\", data-title=\"{2}\", data-frame-css-class=\"{3}\", data-width=\"{4}\", data-height=\"{5}\">",
                asImage,
                html.Encode(previewUrl),
                html.Encode(title),
                frameCssClass,
                width,
                height);
            sb.AppendFormat("<div class=\"bcms-preview-zoom-overlay bcms-js-zoom-overlay\" data-preview-title=\"{0}\" data-preview-url=\"{1}\"> </div>", html.Encode(title), html.Encode(openUrl));
            sb.AppendLine();
            sb.AppendLine("</div>");

            return new MvcHtmlString(sb.ToString());
        }
    }
}