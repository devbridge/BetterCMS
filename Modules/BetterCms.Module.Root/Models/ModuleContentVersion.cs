using System;

using Devbridge.Platform.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ModuleContentVersion : EquatableEntity<ModuleContentVersion>
    {
        public virtual string ModuleName { get; set; }

        public virtual long ContentVersion { get; set; }
    }
}