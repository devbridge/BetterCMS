using BetterCms.Core.Models;

namespace BetterCms.Module.Sitemap.Models.Maps
{
    /// <summary>
    /// The sitemap node entity map.
    /// </summary>
    public class SitemapNodeMap : EntityMapBase<SitemapNode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapNodeMap"/> class.
        /// </summary>
        public SitemapNodeMap() : base(SitemapModuleDescriptor.ModuleName)
        {
            Table("SitemapNodes");

            // Map(x => x.FirstName).Not.Nullable().Length(MaxLength.Name);
            // Map(x => x.LongDescription).Nullable().Length(MaxLength.Max).LazyLoad();
        }
    }
}
