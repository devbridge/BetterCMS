using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PagePropertiesMap : EntitySubClassMapBase<PageProperties>
    {
        public PagePropertiesMap() : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Pages");
            
            Map(x => x.Description).Nullable();
            Map(x => x.CustomJS).Length(int.MaxValue).Nullable();
            Map(x => x.CustomCss).Length(int.MaxValue).Nullable();
            Map(x => x.UseCanonicalUrl).Not.Nullable();
            Map(x => x.UseNoFollow).Not.Nullable();
            Map(x => x.UseNoIndex).Not.Nullable();
            Map(x => x.IsArchived).Not.Nullable();

            
            References(x => x.Image).Cascade.SaveUpdate().LazyLoad();
            References(x => x.SecondaryImage).Cascade.SaveUpdate().LazyLoad();
            References(x => x.FeaturedImage).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.PageTags).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Categories).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");            
        }
    }
}