using System;

namespace BetterCms.Module.Pages.ViewModels.History
{
    public class HtmlContentWidgetHistoryViewModel
    {
        public string Name { get; set; }

        public string Html { get; set; }

        public string CustomCss { get; set; }

        public string CustomJs { get; set; }

        public bool UseCustomCss { get; set; }

        public bool UseCustomJs { get; set; }

        public bool UseCustomHtml { get; set; }
    }
}