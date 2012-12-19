using System;
using System.Linq;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class ServerControlWidget : Widget
    {
        public virtual string Url { get; set; }

        public override Root.Models.Content Clone()
        {
            return new ServerControlWidget
                       {
                           Url = Url,
                           Name = Name,
                           Category = Category,
                           ContentOptions = ContentOptions != null
                                                ? ContentOptions.Select(f => f.Clone()).ToList()
                                                : null
                       };
        }
    }
}