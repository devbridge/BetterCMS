using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Language : EquatableEntity<Language>, ILanguage
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }
    }
}