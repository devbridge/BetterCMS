using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentMap : EntityMapBase<Content>
    {
        public ContentMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Contents");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();

            HasMany(x => x.PageContents).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.ContentOptions).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}