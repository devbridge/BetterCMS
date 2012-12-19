using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class SiteSettingMap : EntityMapBase<SiteSetting>
    {
        public SiteSettingMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("SiteSettings");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Url).Length(MaxLength.Url).Not.Nullable();
            Map(x => x.PrimaryRegionId).Nullable();
            Map(x => x.DefaultLayoutId).Nullable();
            Map(x => x.ImagePath).Length(MaxLength.Url).Nullable();
            Map(x => x.MaxImageWidth).Not.Nullable();
            Map(x => x.AllowComments).Not.Nullable();
            Map(x => x.AllowAnonymousComments).Not.Nullable();
            Map(x => x.TimeZone).Not.Nullable();
            Map(x => x.ImplementsPageComments).Not.Nullable();
            Map(x => x.ImplementsPrivatePages).Not.Nullable();
        }
    }
}