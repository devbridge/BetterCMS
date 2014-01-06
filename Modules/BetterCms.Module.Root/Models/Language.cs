using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Language : EquatableEntity<Language>
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }
    }
}