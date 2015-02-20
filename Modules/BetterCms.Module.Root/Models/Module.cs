using System;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Module : EquatableEntity<Module>
    {
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ModuleVersion { get; set; }

        public virtual bool Enabled { get; set; }
    }
}