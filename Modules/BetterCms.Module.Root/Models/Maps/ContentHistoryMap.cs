using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentHistoryMap : ContentEntityMapBase<ContentHistory>
    {
        public ContentHistoryMap()
            : base(RootModuleDescriptor.ModuleName, "ContentHistory")
        {
            HasMany(x => x.PageContentHistory).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.ContentOptionHistory).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}