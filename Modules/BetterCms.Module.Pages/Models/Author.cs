using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class Author : EquatableEntity<Author>
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string Title { get; set; }
        public virtual string Email { get; set; }
        public virtual string Twitter { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string ProfileThumbnailUrl { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string LongDescription { get; set; }
    }
}