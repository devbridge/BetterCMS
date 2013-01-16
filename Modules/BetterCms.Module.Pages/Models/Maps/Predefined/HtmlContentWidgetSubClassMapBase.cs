using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps.Predefined
{
    public abstract class HtmlContentWidgetSubClassMapBase<THtmlContentWidget> : EntitySubClassMapBase<THtmlContentWidget> where THtmlContentWidget : IHtmlContentWidget
    {
        protected HtmlContentWidgetSubClassMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Html).Not.Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseHtml).Not.Nullable();
            Map(x => x.CustomCss).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.CustomJs).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomJs).Not.Nullable(); 
        }
    }
}
