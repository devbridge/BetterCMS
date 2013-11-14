using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContentWidget : Widget, IHtmlContentWidget, IDynamicContentContainer
    {
        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual bool UseHtml { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }

        public virtual bool EditInSourceMode { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyOptions = true, bool copyRegions = true)
        {
            var copy = (HtmlContentWidget)base.CopyDataTo(content, copyOptions, copyRegions);
            copy.CustomCss = CustomCss;
            copy.UseCustomCss = UseCustomCss;
            copy.Html = Html;
            copy.UseHtml = UseHtml;
            copy.CustomJs = CustomJs;
            copy.UseCustomJs = UseCustomJs;
            copy.EditInSourceMode = EditInSourceMode;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new HtmlContentWidget());
        }
    }
}