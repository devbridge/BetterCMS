// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetaDataProjection.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Renders meta data element (meta).
    /// </summary>
    public class MetaDataProjection : HtmlElementProjection
    {
        private readonly string name;
        private readonly string content;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetaDataProjection" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public MetaDataProjection(string name, string content)
            : base("meta", true)
        {
            this.name = name;
            this.content = content;
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            controlRenderer.Attributes["name"] = name;
            controlRenderer.Attributes["content"] = content;
        }
    }
}
