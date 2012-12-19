using System;

using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class PageCategory : EquatableEntity<PageCategory>
    {
        public virtual Category Category { get; set; }

        public virtual PageProperties Page { get; set; }
    }
}