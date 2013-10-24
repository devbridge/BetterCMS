using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class DynamicHtmlLayoutContent : Root.Models.Content, IDynamicHtmlLayoutContent
    {
        public virtual string Html { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyOptions = true)
        {
            var copy = (DynamicHtmlLayoutContent)base.CopyDataTo(content, copyOptions);
            copy.Html = Html;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new DynamicLayoutContent());
        }
    }
}