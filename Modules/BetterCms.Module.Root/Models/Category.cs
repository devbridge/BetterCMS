using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Category : EquatableEntity<Category>, ICategory
    {
        public virtual string Name { get; set; }
    }
}