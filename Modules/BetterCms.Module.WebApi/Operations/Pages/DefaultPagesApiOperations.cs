using BetterCms.Module.Api.Operations.Pages.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages;
using BetterCms.Module.Api.Operations.Pages.Pages.Page;
using BetterCms.Module.Api.Operations.Pages.Widgets;
using BetterCms.Module.Api.Operations.Pages.Widgets.Widget;

namespace BetterCms.Module.Api.Operations.Pages
{
    public class DefaultPagesApiOperations : IPagesApiOperations
    {
        public DefaultPagesApiOperations(IPagesService pages, IPageService page, IContentService content, IWidgetService widget, IWidgetsService widgets)
        {
            Pages = pages;
            Page = page;
            Content = content;
            Widget = widget;
            Widgets = widgets;
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
        
        public IContentService Content
        {
            get; 
            private set;
        }


        public IWidgetsService Widgets
        {
            get;
            private set;
        }

        public IWidgetService Widget
        {
            get;
            private set;
        }
    }
}
