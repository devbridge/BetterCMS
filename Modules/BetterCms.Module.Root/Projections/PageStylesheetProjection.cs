// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageStylesheetProjection.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageStylesheetProjection : IStylesheetAccessor, ISerializable
    {
        private readonly IPage page;
        private readonly IStylesheetAccessor styleAccessor;

        public PageStylesheetProjection(IPage page, IStylesheetAccessor styleAccessor)
        {
            this.page = page;
            this.styleAccessor = styleAccessor;
        }

        public PageStylesheetProjection(SerializationInfo info, StreamingContext context)
        {
            page = (IPage)info.GetValue("page", typeof(IPage));
            styleAccessor = (IStylesheetAccessor)info.GetValue("styleAccessor", typeof(IStylesheetAccessor));
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("page", page, typeof(IPage));
            info.AddValue("styleAccessor", styleAccessor, styleAccessor.GetType());
        }

        /// <summary>
        /// Gets the custom styles.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Custom style</returns>
        public string[] GetCustomStyles(System.Web.Mvc.HtmlHelper html)
        {
            return styleAccessor.GetCustomStyles(html);
        }

        /// <summary>
        /// Gets the styles resources.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns>Array of style resources</returns>
        public string[] GetStylesResources(System.Web.Mvc.HtmlHelper html)
        {
            return styleAccessor.GetStylesResources(html);
        }
    }
}