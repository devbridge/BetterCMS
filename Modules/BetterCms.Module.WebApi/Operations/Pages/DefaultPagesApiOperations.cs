using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;

namespace BetterCms.Module.Api.Operations.Pages
{
    public class DefaultPagesApiOperations
    {
        public DefaultPagesApiOperations(IPagesService pages, IPageService page)
        {
            Pages = pages;
            Page = page;
        }

        public IPagesService Pages
        {
            get; 
            private set;
        }
        
        public IPageService Page
        {
            get; 
            private set;
        }
    }
}
