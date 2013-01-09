using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContentHistory : Root.Models.ContentHistory, IHtmlContent
    {
        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }      
    }
}