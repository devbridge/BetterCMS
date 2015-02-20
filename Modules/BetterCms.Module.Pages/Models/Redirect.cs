using System;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class Redirect : EquatableEntity<Redirect>
    {
        public virtual string PageUrl { get; set; }
        public virtual string RedirectUrl { get; set; }
    }
}