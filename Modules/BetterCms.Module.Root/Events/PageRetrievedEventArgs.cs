// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageRetrievedEventArgs.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.ViewModels.Cms;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Page retrieved event arguments.
    /// </summary>
    public class PageRetrievedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRetrievedEventArgs" /> class.
        /// </summary>
        /// <param name="renderPageData">The render page data.</param>
        /// <param name="page">The page.</param>
        public PageRetrievedEventArgs(RenderPageViewModel renderPageData, IPage page)
        {
            RenderPageData = renderPageData;
            PageData = page;
        }

        /// <summary>
        /// Gets or sets the rendering page view model.
        /// </summary>
        /// <value>
        /// The rendering page view model.
        /// </value>
        public RenderPageViewModel RenderPageData { get; set; }
        
        /// <summary>
        /// Gets or sets the page entity.
        /// </summary>
        /// <value>
        /// The rendering page entity.
        /// </value>
        public IPage PageData { get; set; }

        /// <summary>
        /// Gets or sets the event result.
        /// </summary>
        /// <value>
        /// The event result.
        /// </value>
        public PageRetrievedEventResult EventResult { get; set; }
    }
}