// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentEvents.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Root.Models;

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
        /// Occurs when any content is inserted to page.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<PageContent>> PageContentInserted;

        /// <summary>
        /// Occurs when any content is removed from page.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<PageContent>> PageContentDeleted;
        
        /// <summary>
        /// Occurs when any content is sorted or moved from one region to another page.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<PageContent>> PageContentSorted;
        
        /// <summary>
        /// Occurs when any content is configured.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<PageContent>> PageContentConfigured;
        
        /// <summary>
        /// Occurs when child content is configured.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<ChildContent>> ChildContentConfigured;
        
        /// <summary>
        /// Occurs when HTML content is created.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<HtmlContent>> HtmlContentCreated;

        /// <summary>
        /// Occurs when HTML content is updated.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<HtmlContent>> HtmlContentUpdated;

        /// <summary>
        /// Occurs when HTML content is deleted.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<HtmlContent>> HtmlContentDeleted;

        /// <summary>
        /// Occurs when content's draft version is destroyed.
        /// </summary>        
        public event DefaultEventHandler<SingleItemEventArgs<Content>> ContentDraftDestroyed;
        
        public void OnPageContentInserted(PageContent pageContent)
        {
            if (PageContentInserted != null)
            {
                PageContentInserted(new SingleItemEventArgs<PageContent>(pageContent));
            }
        }
        
        public void OnPageContentDeleted(PageContent pageContent)
        {
            if (PageContentDeleted != null)
            {
                PageContentDeleted(new SingleItemEventArgs<PageContent>(pageContent));
            }
        }
        
        public void OnPageContentSorted(PageContent pageContent)
        {
            if (PageContentSorted != null)
            {
                PageContentSorted(new SingleItemEventArgs<PageContent>(pageContent));
            }
        }
        
        public void OnPageContentConfigured(PageContent pageContent)
        {
            if (PageContentConfigured != null)
            {
                PageContentConfigured(new SingleItemEventArgs<PageContent>(pageContent));
            }
        }

        public void OnChildContentConfigured(ChildContent childContent)
        {
            if (ChildContentConfigured != null)
            {
                ChildContentConfigured(new SingleItemEventArgs<ChildContent>(childContent));
            }
        }

        public void OnHtmlContentCreated(HtmlContent htmlContent)
        {
            if (HtmlContentCreated != null)
            {
                HtmlContentCreated(new SingleItemEventArgs<HtmlContent>(htmlContent));
            }
        }
        
        public void OnHtmlContentUpdated(HtmlContent htmlContent)
        {
            if (HtmlContentUpdated != null)
            {
                HtmlContentUpdated(new SingleItemEventArgs<HtmlContent>(htmlContent));
            }
        }
        
        public void OnHtmlContentDeleted(HtmlContent htmlContent)
        {
            if (HtmlContentDeleted != null)
            {
                HtmlContentDeleted(new SingleItemEventArgs<HtmlContent>(htmlContent));
            }
        }

        public void OnContentDraftDestroyed(Content content)
        {
            if (ContentDraftDestroyed != null)
            {
                ContentDraftDestroyed(new SingleItemEventArgs<Content>(content));
            }
        }
    }
}
