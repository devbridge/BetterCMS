using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PagePropertiesMap : EntitySubClassMapBase<PageProperties>
    {
        public PagePropertiesMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Pages");
            
            Map(x => x.Description).Nullable();
            Map(x => x.ImageUrl).Nullable();
            Map(x => x.CanonicalUrl).Nullable();
            Map(x => x.CustomCss).Nullable();
            Map(x => x.UseCanonicalUrl).Not.Nullable();
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.UseNoFollow).Not.Nullable();
            Map(x => x.UseNoIndex).Not.Nullable();
            Map(x => x.IsPublic).Not.Nullable();

            References(x => x.Author).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Category).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.PageTags).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.PageCategories).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");             
        }
    }
}