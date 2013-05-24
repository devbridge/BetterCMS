using System;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class DynamicLayoutContent : Root.Models.Content, IDynamicLayoutContent
    {
        public virtual Layout Layout { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content)
        {
            var copy = (DynamicLayoutContent)base.CopyDataTo(content);
            copy.Layout = Layout;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new DynamicLayoutContent());
        }

        ILayout IDynamicLayoutContent.Layout
        {
            get
            {
                return Layout;
            }
        }
    }
}