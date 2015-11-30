// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationProjection.cs" company="Devbridge Group LLC">
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
    /// Renders relation element (link).
    /// </summary>
    public class RelationProjection : HtmlElementProjection
    {
        private readonly string relation;
        private readonly string type;
        private readonly string link;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationProjection" /> class.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="link">The link.</param>
        public RelationProjection(string relation, string link) : this(relation, link, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelationProjection" /> class.
        /// </summary>
        /// <param name="relation">The relation.</param>
        /// <param name="link">The link.</param>
        /// <param name="type">The type.</param>
        public RelationProjection(string relation, string link, string type)
            : base("link", true)
        {
            this.relation = relation;
            this.type = type;
            this.link = link;
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, HtmlHelper html)
        {
            controlRenderer.Attributes["rel"] = relation;
            controlRenderer.Attributes["href"] = link;
            if (!string.IsNullOrWhiteSpace(type))
            {
                controlRenderer.Attributes["type"] = type;
            }
        }
    }
}
