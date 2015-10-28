using System;

namespace BetterCms.Module.Pages.ViewModels.History
{
    public class HtmlContentHistoryViewModel
    {
        public string Name { get; set; }

        public DateTime ActivationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string RowText { get; set; }

        public string CustomCss { get; set; }

        public string CustomJs { get; set; }

        public bool UseCustomCss { get; set; }

        public bool UseCustomJs { get; set; }
    }
}