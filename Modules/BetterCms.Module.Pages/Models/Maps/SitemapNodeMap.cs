using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    /// <summary>
    /// The sitemap node entity map.
    /// </summary>
    public class SitemapNodeMap : EntityMapBase<SitemapNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapNodeMap"/> class.
        /// </summary>
        public SitemapNodeMap() : base(PagesModuleDescriptor.ModuleName)
        {
            Table("SitemapNodes");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.UsePageTitleAsNodeTitle).Not.Nullable();
            Map(x => x.Url).Nullable().Length(MaxLength.Url);
            Map(x => x.UrlHash).Nullable().Length(MaxLength.UrlHash);
            Map(x => x.DisplayOrder).Not.Nullable();
            Map(x => x.Macro).Nullable().Length(MaxLength.Text);

            References(x => x.Sitemap).Cascade.SaveUpdate().LazyLoad();
            References(f => f.ParentNode).Cascade.SaveUpdate().Nullable().LazyLoad();
            HasMany(f => f.ChildNodes).Table("SitemapNodes").KeyColumn("ParentNodeId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0").LazyLoad();
            HasMany(f => f.Translations).Table("SitemapNodeTranslations").KeyColumn("NodeId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0").LazyLoad();

            References(x => x.Page).Cascade.None().LazyLoad();
        }
    }
}
