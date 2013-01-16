using System;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class Option : EquatableEntity<Option>
    {
        public virtual Layout DefaultLayout { get; set; }
    }
}