using System;

using BetterCms.Module.Root.Models;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageTag : EquatableEntity<PageTag>
    {
        public virtual Tag Tag { get; set; }
        public virtual PageProperties Page { get; set; }
    }
}