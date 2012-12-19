using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class HtmlControlWidgetMap : EntitySubClassMapBase<HtmlContentWidget>
    {
        public HtmlControlWidgetMap()
            : base(PagesModuleDescriptor.ModuleName)
        {
            Table("HtmlContentWidgets");
            
            Map(x => x.Html).Not.Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseHtml).Not.Nullable();
            Map(x => x.CustomCss).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.CustomJs).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomJs).Not.Nullable(); 
        }
    }
}
