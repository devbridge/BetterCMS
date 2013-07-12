using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.ViewModels.Cms;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents : EventsBase<RootEvents>
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
        public PageRetrievedEventResult OnPageRetrieved(RenderPageViewModel renderPageData, IPage pageData)
        {
            if (PageRetrieved != null)
            {
                var args = new PageRetrievedEventArgs(renderPageData, pageData);
                PageRetrieved(args);
                return args.EventResult;
            }

            return PageRetrievedEventResult.None;
        }
    }
}
