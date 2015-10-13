using System;
using BetterCms.Module.MediaManager.Models;
using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class Author : EquatableEntity<Author>{
        public virtual string Name { get; set; }

        public virtual MediaImage Image { get; set; }

        public virtual string Description { get; set; }
    }
}