// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SitemapEvents.cs" company="Devbridge Group LLC">
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
    /// Attachable sitemap events container
    /// </summary>
    public partial class SitemapEvents : EventsBase<SitemapEvents>
    {
        /// <summary>
        /// Occurs when a sitemap is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Sitemap>> SitemapCreated;

        /// <summary>
        /// Occurs when a sitemap is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Sitemap>> SitemapUpdated;

        /// <summary>
        /// Occurs when a sitemap is restored.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Sitemap>> SitemapRestored;

        /// <summary>
        /// Occurs when a sitemap is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Sitemap>> SitemapDeleted;

        /// <summary>
        /// Occurs when a sitemap node is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<SitemapNode>> SitemapNodeCreated;

        /// <summary>
        /// Occurs when a sitemap node is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<SitemapNode>> SitemapNodeUpdated;

        /// <summary>
        /// Occurs when a sitemap node is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<SitemapNode>> SitemapNodeDeleted;

        /// <summary>
        /// Called when the sitemap is created.
        /// </summary>
        public void OnSitemapCreated(Sitemap sitemap)
        {
            if (SitemapCreated != null)
            {
                SitemapCreated(new SingleItemEventArgs<Sitemap>(sitemap));
            }
        }

        /// <summary>
        /// Called when the sitemap is updated.
        /// </summary>
        public void OnSitemapUpdated(Sitemap sitemap)
        {
            if (SitemapUpdated != null)
            {
                SitemapUpdated(new SingleItemEventArgs<Sitemap>(sitemap));
            }
        }

        /// <summary>
        /// Called when the sitemap is restored.
        /// </summary>
        public void OnSitemapRestored(Sitemap sitemap)
        {
            if (SitemapRestored != null)
            {
                SitemapRestored(new SingleItemEventArgs<Sitemap>(sitemap));
            }
        }

        /// <summary>
        /// Called when the sitemap is deleted.
        /// </summary>
        public void OnSitemapDeleted(Sitemap sitemap)
        {
            if (SitemapDeleted != null)
            {
                SitemapDeleted(new SingleItemEventArgs<Sitemap>(sitemap));
            }
        }

        /// <summary>
        /// Called when sitemap node is created.
        /// </summary>
        public void OnSitemapNodeCreated(SitemapNode node)
        {
            if (SitemapNodeCreated != null)
            {
                SitemapNodeCreated(new SingleItemEventArgs<SitemapNode>(node));
            }
        }

        /// <summary>
        /// Called when sitemap node is updated.
        /// </summary>
        public void OnSitemapNodeUpdated(SitemapNode node)
        {
            if (SitemapNodeUpdated != null)
            {
                SitemapNodeUpdated(new SingleItemEventArgs<SitemapNode>(node));
            }
        }

        /// <summary>
        /// Called when sitemap node is deleted.
        /// </summary>
        public void OnSitemapNodeDeleted(SitemapNode node)
        {
            if (SitemapNodeDeleted != null)
            {
                SitemapNodeDeleted(new SingleItemEventArgs<SitemapNode>(node));
            }
        }
    }
}
