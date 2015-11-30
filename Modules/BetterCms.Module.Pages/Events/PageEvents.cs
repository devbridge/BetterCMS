// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageEvents.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Events;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PageEvents : EventsBase<PageEvents>
    {        
        /// <summary>
        /// Occurs when page is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageCreated;
        
        /// <summary>
        /// Occurs when page is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageDeleted;
        
        /// <summary>
        /// Occurs when a page cloned.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageCloned;

        /// <summary>
        /// Occurs before page properties are changed.
        /// </summary>
        public event DefaultEventHandler<PagePropertiesChangingEventArgs> PagePropertiesChanging;

        /// <summary>
        /// Occurs when a page properties is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PagePropertiesChanged;
 
        /// <summary>
        /// Occurs when a page publish status is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PagePublishStatusChanged;

        /// <summary>
        /// Occurs when a page SEO status is changed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<PageProperties>> PageSeoStatusChanged;
         
        /// <summary>
        /// Called when page is created.
        /// </summary>
        public void OnPageCreated(PageProperties page)
        {
            if (PageCreated != null)
            {
                PageCreated(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        /// <summary>
        /// Called when page is deleted.
        /// </summary>
        public void OnPageDeleted(PageProperties page)
        {
            if (PageDeleted != null)
            {
                PageDeleted(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public void OnPageCloned(PageProperties page)
        {
            if (PageCloned != null)
            {
                PageCloned(new SingleItemEventArgs<PageProperties>(page));                
            }
        }

        public void OnPagePropertiesChanged(PageProperties page)
        {
            if (PagePropertiesChanged != null)
            {
                PagePropertiesChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public PagePropertiesChangingEventArgs OnPagePropertiesChanging(UpdatingPagePropertiesModel beforeUpdate, UpdatingPagePropertiesModel afterUpdate)
        {
            var args = new PagePropertiesChangingEventArgs(beforeUpdate, afterUpdate);
            
            if (PagePropertiesChanging != null)
            {
                PagePropertiesChanging(args);
            }

            return args;
        }

        public void OnPagePublishStatusChanged(PageProperties page)
        {
            if (PagePublishStatusChanged != null)
            {
                PagePublishStatusChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }

        public void OnPageSeoStatusChanged(PageProperties page)
        {
            if (PageSeoStatusChanged != null)
            {
                PageSeoStatusChanged(new SingleItemEventArgs<PageProperties>(page));
            }
        }
    }
}
