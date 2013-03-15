using System;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Api.Events
{
    /// <summary>
    /// Page rendering event arguments.
    /// </summary>
    public class PageRenderingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRenderingEventArgs" /> class.
        /// </summary>
        /// <param name="renderPageData">The render page data.</param>
        public PageRenderingEventArgs(RenderPageViewModel renderPageData)
        {
            RenderPageData = renderPageData;
        }

        /// <summary>
        /// Gets or sets the sitemap node where page is placed.
        /// </summary>
        /// <value>
        /// The sitemap node.
        /// </value>
        public RenderPageViewModel RenderPageData { get; set; }
    }
}
