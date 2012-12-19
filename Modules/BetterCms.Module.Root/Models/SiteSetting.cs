using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class SiteSetting : EquatableEntity<SiteSetting>
    {
        public virtual string Name { get; set; }
        public virtual string Url { get; set; }
        public virtual Guid? PrimaryRegionId { get; set; }
        public virtual Guid? DefaultLayoutId { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual int MaxImageWidth { get; set; }
        public virtual bool AllowComments { get; set; }
        public virtual bool AllowAnonymousComments { get; set; }
        public virtual int TimeZone { get; set; }
        public virtual bool ImplementsPageComments { get; set; }
        public virtual bool ImplementsPrivatePages { get; set; }
    }
}