using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    /// <summary>
    /// The sitemap node entity map.
    /// </summary>
    public class SitemapMap : EntityMapBase<Sitemap>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapMap"/> class.
        /// </summary>
        public SitemapMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Sitemaps");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            HasMany(f => f.Nodes).Table("SitemapNodes").KeyColumn("SitemapId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0");
            HasMany(x => x.SitemapTags).KeyColumn("SitemapId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");

            HasManyToMany(x => x.AccessRules).Table("SitemapAccessRules").Schema(SchemaName).Cascade.AllDeleteOrphan().LazyLoad();
        }
    }
}
