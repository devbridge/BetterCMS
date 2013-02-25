using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class Redirect : EquatableEntity<Redirect>, IRedirect
    {
        public virtual string PageUrl { get; set; }
        public virtual string RedirectUrl { get; set; }
    }
}