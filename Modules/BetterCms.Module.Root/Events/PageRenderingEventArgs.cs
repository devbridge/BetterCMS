using System;

using BetterCms.Module.Root.ViewModels.Cms;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
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
        /// Gets or sets the render page view model.
        /// </summary>
        /// <value>
        /// The render page view model.
        /// </value>
        public RenderPageViewModel RenderPageData { get; set; }
    }
}
