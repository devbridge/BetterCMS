// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootEvents.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents : EventsBase<RootEvents>
    {                        
        /// <summary>
        /// Occurs when a page is rendering.
        /// </summary>
        public event DefaultEventHandler<PageRenderingEventArgs> PageRendering;
        
        /// <summary>
        /// Occurs when a page is retrieved.
        /// </summary>
        public event DefaultEventHandler<PageRetrievedEventArgs> PageRetrieved;

        /// <summary>
        /// Occurs when page was not found by virtual path.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<string>> PageNotFound;

        /// <summary>
        /// Occurs when page access was forbidden.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<RenderPageViewModel>> PageAccessForbidden;

        /// <summary>
        /// Called when page is rendering.
        /// </summary>
        /// <param name="renderPageData">The rendering page data.</param>
        public void OnPageRendering(RenderPageViewModel renderPageData)
        {
            if (PageRendering != null)
            {
                PageRendering(new PageRenderingEventArgs(renderPageData));
            }
        }

        /// <summary>
        /// Called when page is retrieved.
        /// </summary>
        /// <param name="renderPageData">The rendering page data.</param>
        /// <param name="pageData">The page data.</param>
        public PageRetrievedEventResult OnPageRetrieved(RenderPageViewModel renderPageData, IPage pageData)
        {
            if (PageRetrieved != null)
            {
                var args = new PageRetrievedEventArgs(renderPageData, pageData);
                PageRetrieved(args);
                return args.EventResult;
            }

            return PageRetrievedEventResult.None;
        }

        /// <summary>
        /// Called when page was not found.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public void OnPageNotFound(string virtualPath)
        {
            if (PageNotFound != null)
            {
                PageNotFound(new SingleItemEventArgs<string>(virtualPath));
            }
        }

        /// <summary>
        /// Called when page access is forbidden.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        public void OnPageAccessForbidden(RenderPageViewModel pageModel)
        {
            if (PageAccessForbidden != null)
            {
                PageAccessForbidden(new SingleItemEventArgs<RenderPageViewModel>(pageModel));
            }
        }
    }
}
