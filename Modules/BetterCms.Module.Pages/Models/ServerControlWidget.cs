using System;
using System.Linq;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class ServerControlWidget : Widget, IServerControlWidget
    {
        public virtual string Url { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content)
        {
            var copy = (ServerControlWidget)base.CopyDataTo(content);
            copy.Url = Url;

            return copy;
        }

        public override Root.Models.Content Clone()
        {
            return CopyDataTo(new ServerControlWidget());
        }
    }
}