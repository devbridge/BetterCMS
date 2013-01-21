using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Blog.Models
{
    [Serializable]
    public class BlogPost : PageProperties
    {
        public virtual Author Author { get; set; }
    }
}