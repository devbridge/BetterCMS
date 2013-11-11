using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContent : Root.Models.Content, IHtmlContent, IDynamicContentContainer
    {
        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }
        
        public virtual bool EditInSourceMode { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyOptions = true, bool copyRegions = true)
        {
            var copy = (HtmlContent)base.CopyDataTo(content, copyOptions, copyRegions);
            copy.ActivationDate = ActivationDate;
            copy.ExpirationDate = ExpirationDate;
            copy.CustomCss = CustomCss;
            copy.UseCustomCss = UseCustomCss;
            copy.Html = Html;            
            copy.CustomJs = CustomJs;
            copy.UseCustomJs = UseCustomJs;
            copy.EditInSourceMode = EditInSourceMode;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new HtmlContent());
        }
    }
}