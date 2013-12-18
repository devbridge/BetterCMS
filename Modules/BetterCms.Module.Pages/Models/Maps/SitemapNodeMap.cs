using BetterCms.Core.Models;

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
            Map(x => x.Url).Nullable().Length(MaxLength.Url);
            Map(x => x.DisplayOrder).Not.Nullable();

            References(x => x.Sitemap).Cascade.SaveUpdate().LazyLoad();
            References(f => f.ParentNode).Cascade.SaveUpdate().Nullable();
            HasMany(f => f.ChildNodes).Table("SitemapNodes").KeyColumn("ParentNodeId").Inverse().Cascade.SaveUpdate().Where("IsDeleted = 0");

            References(x => x.Page).Cascade.SaveUpdate().LazyLoad();
        }
    }
}
