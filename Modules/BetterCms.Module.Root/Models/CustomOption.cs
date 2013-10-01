using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class CustomOption : EquatableEntity<CustomOption>, ICustomOption
    {
        public virtual string Title { get; set; }

        public virtual string Identifier { get; set; }
    }
}