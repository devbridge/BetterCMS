using System;

namespace BetterCms.Module.Pages.Models
{
    public interface IHtmlContent
    {
        string CustomCss { get; set; }

        bool UseCustomCss { get; set; }

        string Html { get; set; }

        string CustomJs { get; set; }

        bool UseCustomJs { get; set; }   
    }
}