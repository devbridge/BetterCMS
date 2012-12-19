using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContent : Root.Models.Content, IHtmlContent
    {
        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }

        public override Root.Models.Content Clone()
        {
            return new HtmlContent
                {
                    Name = Name,
                    ActivationDate = ActivationDate,
                    ExpirationDate = ExpirationDate,
                    CustomCss = CustomCss,
                    UseCustomCss = UseCustomCss,
                    CustomJs = CustomJs,
                    UseCustomJs = UseCustomJs,
                    Html = Html
                };
        }
    }
}