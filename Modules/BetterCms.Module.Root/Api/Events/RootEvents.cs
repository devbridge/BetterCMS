using BetterCms.Api;
using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Api.Events
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootApiEvents : EventsBase
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
        public void OnPageRetrieved(RenderPageViewModel renderPageData, IPage pageData)
        {
            if (PageRetrieved != null)
            {
                PageRetrieved(new PageRetrievedEventArgs(renderPageData, pageData));
            }
        }
    }
}
