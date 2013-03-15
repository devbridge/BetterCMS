using System;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class Author : EquatableEntity<Author>{
        public virtual string Name { get; set; }

        public virtual MediaImage Image { get; set; }
    }
}