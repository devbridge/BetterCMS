// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RedirectEvents.cs" company="Devbridge Group LLC">
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

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class PageEvents
    {
        /// <summary>
        /// Occurs when a redirect is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectCreated;

        /// <summary>
        /// Occurs when a redirect is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectUpdated;

        /// <summary>
        /// Occurs when a redirect is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Redirect>> RedirectDeleted; 
 
        public void OnRedirectCreated(Redirect redirect)
        {
            if (RedirectCreated != null)
            {
                RedirectCreated(new SingleItemEventArgs<Redirect>(redirect));
            }
        }

        public void OnRedirectUpdated(Redirect redirect)
        {
            if (RedirectUpdated != null)
            {
                RedirectUpdated(new SingleItemEventArgs<Redirect>(redirect));
            }
        }

        public void OnRedirectDeleted(Redirect redirect)
        {
            if (RedirectDeleted != null)
            {
                RedirectDeleted(new SingleItemEventArgs<Redirect>(redirect));
            }        
        }
    }
}
