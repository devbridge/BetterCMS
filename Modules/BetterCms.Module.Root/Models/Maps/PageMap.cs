using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageMap : EntityMapBase<Page>
    {
        public PageMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Pages");

            Map(x => x.PageUrl).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.PageUrlLowerTrimmed).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Status).Not.Nullable();
            Map(x => x.PublishedOn).Nullable();
            Map(x => x.MetaTitle).Length(MaxLength.Name);
            Map(x => x.MetaKeywords).Length(MaxLength.Max);
            Map(x => x.MetaDescription).Length(MaxLength.Max);

            References(x => x.Layout).Not.Nullable().Cascade.SaveUpdate().LazyLoad();
            
            HasMany(x => x.PageContents).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Options).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
