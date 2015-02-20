using BetterCms.Core.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlContentWidgetMap : EntitySubClassMapBase<HtmlContentWidget>
    {
        public HtmlContentWidgetMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("HtmlContentWidgets");

            Map(x => x.Html).Not.Nullable().Length(int.MaxValue).CustomType<EncryptableString>();
            Map(x => x.UseHtml).Not.Nullable();
            Map(x => x.CustomCss).Nullable().Length(int.MaxValue);
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.CustomJs).Nullable().Length(int.MaxValue);
            Map(x => x.UseCustomJs).Not.Nullable();
            Map(x => x.EditInSourceMode).Not.Nullable();             
        }
    }
}
