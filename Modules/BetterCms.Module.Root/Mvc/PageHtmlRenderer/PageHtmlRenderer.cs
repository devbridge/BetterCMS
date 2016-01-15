// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageHtmlRenderer.cs" company="Devbridge Group LLC">
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
using System.Text;
using System.Text.RegularExpressions;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    /// <summary>
    /// Helper class, helps to render page contents HTML
    /// </summary>
    public class PageHtmlRenderer
    {
        /// <summary>
        /// The model
        /// </summary>
        private readonly RenderPageViewModel model;

        /// <summary>
        /// The HTML string builder
        /// </summary>
        private StringBuilder stringBuilder;

        /// <summary>
        /// The rendering page properties
        /// </summary>
        private readonly static IDictionary<string, IRenderingPageProperty> properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHtmlRenderer" /> class.
        /// </summary>
        /// <param name="stringBuilder">The string builder.</param>
        /// <param name="model">The model.</param>
        public PageHtmlRenderer(StringBuilder stringBuilder, RenderPageViewModel model)
        {
            this.model = model;
            this.stringBuilder = stringBuilder;
        }

        /// <summary>
        /// Initializes the <see cref="PageHtmlRenderer" /> class.
        /// </summary>
        static PageHtmlRenderer()
        {
            properties = new Dictionary<string, IRenderingPageProperty>();

            Register(new RenderingPageTitleProperty());
            Register(new RenderingPageUrlProperty());
            Register(new RenderingPageMetaTitleProperty());
            Register(new RenderingPageMetaKeywordsProperty());
            Register(new RenderingPageMetaDescriptionProperty());
            Register(new RenderingPageOptionProperty());
            Register(new RenderingPageCreatedOnProperty());
            Register(new RenderingPageModifiedOnProperty());
            Register(new RenderingPageIdProperty());
        }

        /// <summary>
        /// Registers the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        public static void Register(IRenderingPageProperty property)
        {
            if (!properties.ContainsKey(property.Identifier))
            {
                properties.Add(property.Identifier, property);
            }
        }

        /// <summary>
        /// Replaces HTML with data from page view model.
        /// </summary>
        /// <returns>Replaced HTML</returns>
        public StringBuilder GetReplacedHtml()
        {
            foreach (var property in properties)
            {
                stringBuilder = property.Value.GetReplacedHtml(stringBuilder, model);
            }

            return stringBuilder;
        }

        /// <summary>
        /// Replaces the region representation HTML.
        /// </summary>
        public void ReplaceRegionRepresentationHtml()
        {
            var html = ReplaceRegionRepresentationHtml(stringBuilder.ToString(), "<div class=\"bcms-draggable-region\">Content to add</div>");
            stringBuilder = new StringBuilder(html);
        }

        /// <summary>
        /// Replaces the region representation HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="replaceWith">The replace with.</param>
        public static string ReplaceRegionRepresentationHtml(string html, string replaceWith)
        {
           return Regex.Replace(
                html,
                RootModuleConstants.DynamicRegionRegexPattern,
                EscapeReplacement(replaceWith),
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Replaces the given HTML within master page with region identifier.
        /// </summary>
        /// <param name="regionIdentifier">The region id.</param>
        /// <param name="replaceWith">The replace with.</param>
        public void ReplaceRegionHtml(string regionIdentifier, string replaceWith)
        {
            var replacement = string.Format(RootModuleConstants.DynamicRegionReplacePattern, regionIdentifier);

            stringBuilder = new StringBuilder(Regex.Replace(stringBuilder.ToString(), replacement, EscapeReplacement(replaceWith), RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Escapes the replacement.
        /// </summary>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        private static string EscapeReplacement(string replacement)
        {
            return replacement.Replace("$", "$$");
        }
    }
}