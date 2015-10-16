using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageMap : EntityMapBase<Page>
    {
        public PageMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Pages");

            Map(x => x.PageUrl).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.PageUrlHash).Not.Nullable().Length(MaxLength.UrlHash);
            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Status).Not.Nullable();
            Map(x => x.PublishedOn).Nullable();
            Map(x => x.MetaTitle).Length(MaxLength.Name);
            Map(x => x.MetaKeywords).Length(MaxLength.Max);
            Map(x => x.MetaDescription).Length(MaxLength.Max);
            Map(x => x.IsMasterPage).Not.Nullable();
            Map(x => x.LanguageGroupIdentifier).Nullable();
            Map(x => x.ForceAccessProtocol).Not.Nullable();

            References(x => x.Layout).Nullable().Cascade.SaveUpdate().LazyLoad();
            References(x => x.MasterPage).Nullable().Cascade.SaveUpdate().LazyLoad();
            References(x => x.Language).Nullable().Cascade.SaveUpdate().LazyLoad();

            References(x => x.PagesView).Column("Id").ReadOnly();
            
            HasMany(x => x.PageContents).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Options).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.MasterPages).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");

            HasManyToMany(x => x.AccessRules).Table("PageAccessRules").Schema(SchemaName).Cascade.AllDeleteOrphan().LazyLoad();           
        }
    }
}
