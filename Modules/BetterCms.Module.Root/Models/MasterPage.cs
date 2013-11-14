using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class MasterPage : EquatableEntity<MasterPage>
    {
        public virtual Page Page { get; set; }

        public virtual Page Master { get; set; }
    }
}