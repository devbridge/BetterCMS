using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Models;

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
    }
}
