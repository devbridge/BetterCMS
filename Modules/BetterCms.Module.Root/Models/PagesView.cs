using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class PagesView : EquatableEntity<PagesView>
    {
        public virtual Page Page { get; set; }

        public virtual bool IsInSitemap { get; set; }
    }
}