using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContentWidget : Widget, IHtmlContentWidget
    {
        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual bool UseHtml { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }

        public override Root.Models.Content Clone()
        {
            return new HtmlContentWidget
            {
                Name = Name,
                Category = Category,
                CustomCss = CustomCss,
                UseCustomCss = UseCustomCss,
                CustomJs = CustomJs,
                UseCustomJs = UseCustomJs,
                Html = Html,
                UseHtml = UseHtml
            };
        }
    }
}