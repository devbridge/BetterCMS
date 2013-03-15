using BetterCms.Api;
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
       
        public void OnPageRendering(RenderPageViewModel renderPageData)
        {
            if (PageRendering != null)
            {
                PageRendering(new PageRenderingEventArgs(renderPageData));
            }
        }
    }
}
