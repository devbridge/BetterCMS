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

        public override Root.Models.Content CopyDataTo(Root.Models.Content content)
        {
            var copy = (HtmlContent)base.CopyDataTo(content);
            copy.ActivationDate = ActivationDate;
            copy.ExpirationDate = ExpirationDate;
            copy.CustomCss = CustomCss;
            copy.UseCustomCss = UseCustomCss;
            copy.Html = Html;            
            copy.CustomJs = CustomJs;
            copy.UseCustomJs = UseCustomJs;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new HtmlContent());
        }
    }
}