using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Tag : EquatableEntity<Tag>, ITag
    {
        public virtual string Name { get; set; }
    }
}