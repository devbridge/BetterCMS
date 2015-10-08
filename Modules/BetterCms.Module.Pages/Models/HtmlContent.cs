using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.Models.Enums;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContent : Root.Models.Content, IHtmlContent, IDynamicContentContainer
    {
        public HtmlContent()
        {
            ContentTextMode = ContentTextMode.Html;
        }

        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual string OriginalText { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }
        
        public virtual bool EditInSourceMode { get; set; }

        public virtual ContentTextMode ContentTextMode { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyCollections = true)
        {
            var copy = (HtmlContent)base.CopyDataTo(content, copyCollections);
            copy.ActivationDate = ActivationDate;
            copy.ExpirationDate = ExpirationDate;
            copy.CustomCss = CustomCss;
            copy.UseCustomCss = UseCustomCss;
            copy.Html = Html;            
            copy.CustomJs = CustomJs;
            copy.UseCustomJs = UseCustomJs;
            copy.EditInSourceMode = EditInSourceMode;
            copy.ContentTextMode = ContentTextMode;
            copy.OriginalText = OriginalText;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new HtmlContent());
        }
    }
}