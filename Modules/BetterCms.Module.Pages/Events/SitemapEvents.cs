using System;
using System.Collections.Generic;

using BetterCms.Module.Pages.Models;

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
        /// Occurs when a sitemap is updated.
        /// </summary>
        public event DefaultEventHandler<EventArgs> SitemapUpdated;

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
        /// Called when sitemap is updated.
        /// </summary>
        public void OnSitemapUpdated()
        {
            if (SitemapUpdated != null)
            {
                SitemapUpdated(EventArgs.Empty);
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
