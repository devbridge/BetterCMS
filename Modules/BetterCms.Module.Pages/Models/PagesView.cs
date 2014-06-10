using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PagesView : EquatableEntity<PagesView>
    {
        public virtual Page Page { get; set; }

        public virtual bool IsInSitemap { get; set; }
    }
}