// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkActionProjection.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

using BetterModules.Core.Web.Modules;

namespace BetterCms.Core.Modules.Projections
{
    /// <summary>
    /// Defines link rendering logic with attached properties to call an action in the JavaScript module.
    /// </summary>
    public class LinkActionProjection : ActionCallProjectionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkActionProjection" /> class.
        /// </summary>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        public LinkActionProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> onClickAction)
            : base("a", parentModuleInclude, onClickAction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkActionProjection" /> class.
        /// </summary>
        /// <param name="parentModuleInclude">The parent module.</param>
        /// <param name="title">Link title.</param>
        /// <param name="onClickAction">Name of the action to execute after button click.</param>
        public LinkActionProjection(JsIncludeDescriptor parentModuleInclude, Func<IPage, string> title, Func<IPage, string> onClickAction)
            : base("a", parentModuleInclude, title, onClickAction)
        {
        }

        /// <summary>
        /// Called before render methods sends element to response output.
        /// </summary>
        /// <param name="controlRenderer">The html control renderer.</param>
        /// <param name="page">The page.</param>
        /// <param name="html">The html helper.</param>
        protected override void OnPreRender(HtmlControlRenderer controlRenderer, IPage page, System.Web.Mvc.HtmlHelper html)
        {
            base.OnPreRender(controlRenderer, page, html);

            controlRenderer.Attributes.Add("href", "#");
        }
    }
}
