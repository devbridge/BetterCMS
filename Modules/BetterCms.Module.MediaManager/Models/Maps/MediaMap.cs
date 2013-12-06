using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    [Serializable]
    public class MediaMap : EntityMapBase<Media>
    {
        public MediaMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("Medias");

            Map(x => x.Title).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Description).Length(MaxLength.Text).Nullable();
            Map(x => x.IsArchived).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.ContentType).Not.Nullable();
            Map(x => x.PublishedOn).Nullable();

            References(f => f.Folder).Cascade.SaveUpdate().LazyLoad().Nullable();
            References(f => f.Original).Cascade.SaveUpdate().LazyLoad().Nullable();

            References(f => f.Image).Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.MediaTags).KeyColumn("MediaId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.History).KeyColumn("OriginalId").Cascade.None().LazyLoad().Where("IsDeleted = 0");
        }
    }
}