using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentContentMap : EntityMapBase<ChildContent>
    {
        public ContentContentMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ChildContents");

            References(f => f.Parent).Column("ParentContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
            References(f => f.Child).Column("ChildContentId").Cascade.SaveUpdate().LazyLoad().Not.Nullable();
        }
    }
}
