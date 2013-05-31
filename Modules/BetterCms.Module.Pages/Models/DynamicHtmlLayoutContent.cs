using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class DynamicHtmlLayoutContent : Root.Models.Content, IDynamicHtmlLayoutContent
    {
        public virtual string Html { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content)
        {
            var copy = (DynamicHtmlLayoutContent)base.CopyDataTo(content);
            copy.Html = Html;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new DynamicLayoutContent());
        }
    }
}