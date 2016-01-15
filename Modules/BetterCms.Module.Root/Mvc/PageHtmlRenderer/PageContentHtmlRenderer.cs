// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentHtmlRenderer.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Mvc.PageHtmlRenderer
{
    /// <summary>
    /// Helper class, helps to render page content HTML
    /// </summary>
    public static class PageContentHtmlRenderer
    {
        /// <summary>
        /// The rendering page properties
        /// </summary>
        private readonly static IDictionary<string, IRenderingProperty> properties;

        /// <summary>
        /// Initializes the <see cref="PageHtmlRenderer" /> class.
        /// </summary>
        static PageContentHtmlRenderer()
        {
            properties = new Dictionary<string, IRenderingProperty>();

            Register(new RenderingContentOptionProperty());
        }

        /// <summary>
        /// Registers the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        public static void Register(IRenderingProperty property)
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
        public static StringBuilder GetReplacedHtml(StringBuilder stringBuilder, IList<IOptionValue> values)
        {
            foreach (var property in properties)
            {
                var renderingOption = property.Value as IRenderingOption;
                if (renderingOption != null)
                {
                    stringBuilder = renderingOption.GetReplacedHtml(stringBuilder, values);
                }
            }

            return stringBuilder;
        }
    }
}