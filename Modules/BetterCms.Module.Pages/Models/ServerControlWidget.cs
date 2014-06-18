using System;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class ServerControlWidget : Widget, IServerControlWidget
    {
        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyCollections = true)
        {
            var copy = (ServerControlWidget)base.CopyDataTo(content, copyCollections);
            copy.Url = Url;

            return copy;
        }

        public virtual string Url { get; set; }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new ServerControlWidget());
        }
    }
}