using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Events;

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
        /// Occurs when page was not found by virtual path.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<string>> PageNotFound;

        /// <summary>
        /// Occurs when page access was forbidden.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<RenderPageViewModel>> PageAccessForbidden;

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

        /// <summary>
        /// Called when page was not found.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        public void OnPageNotFound(string virtualPath)
        {
            if (PageNotFound != null)
            {
                PageNotFound(new SingleItemEventArgs<string>(virtualPath));
            }
        }

        /// <summary>
        /// Called when page access is forbidden.
        /// </summary>
        /// <param name="pageModel">The page model.</param>
        public void OnPageAccessForbidden(RenderPageViewModel pageModel)
        {
            if (PageAccessForbidden != null)
            {
                PageAccessForbidden(new SingleItemEventArgs<RenderPageViewModel>(pageModel));
            }
        }
    }
}
