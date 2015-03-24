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
