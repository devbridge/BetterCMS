using BetterCms.Core.Models;

namespace BetterCms.Module.Navigation.Models.Maps
{
    /// <summary>
    /// The sitemap node entity map.
    /// </summary>
    public class SitemapNodeMap : EntityMapBase<SitemapNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapNodeMap"/> class.
        /// </summary>
        public SitemapNodeMap() : base(NavigationModuleDescriptor.ModuleName)
        {
            Table("SitemapNodes");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Url).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.DisplayOrder).Not.Nullable();

            References(f => f.ParentNode).Cascade.SaveUpdate().LazyLoad().Nullable();
            HasMany(f => f.ChildNodes).KeyColumn("ParentNodeId").Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}
