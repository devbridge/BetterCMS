using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    /// <summary>
    /// The sitemap node entity map.
    /// </summary>
    public class SitemapNodeTranslationMap : EntityMapBase<SitemapNodeTranslation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapNodeTranslationMap"/> class. 
        /// </summary>
        public SitemapNodeTranslationMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("SitemapNodeTranslations");

            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.UsePageTitleAsNodeTitle).Not.Nullable();
            Map(x => x.Url).Nullable().Length(MaxLength.Url);
            Map(x => x.UrlHash).Nullable().Length(MaxLength.UrlHash);
            Map(x => x.Macro).Nullable().Length(MaxLength.Text);

            References(x => x.Node).Not.Nullable().Cascade.SaveUpdate().LazyLoad();
            References(x => x.Language).Not.Nullable().Cascade.SaveUpdate().LazyLoad();
        }
    }
}
