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