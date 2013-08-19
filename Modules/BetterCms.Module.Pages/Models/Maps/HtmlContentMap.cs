using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentMap : EntitySubClassMapBase<HtmlContent>
    {
        public HtmlContentMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("HtmlContents");

            Map(x => x.ActivationDate).Not.Nullable();
            Map(x => x.ExpirationDate).Nullable();
            Map(x => x.Html).Not.Nullable().Length(int.MaxValue).CustomType<EncryptableString>();
            Map(x => x.CustomCss).Nullable().Length(int.MaxValue);
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.CustomJs).Nullable().Length(int.MaxValue);
            Map(x => x.UseCustomJs).Not.Nullable();
            Map(x => x.EditInSourceMode).Not.Nullable(); 
        }
    }
}
