using System;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class Author : EquatableEntity<Author>, IAuthor
    {
        public virtual string Name { get; set; }

        public virtual MediaImage Image { get; set; }

        IMediaImage IAuthor.Image
        {
            get { return Image; }
        }
    }
}