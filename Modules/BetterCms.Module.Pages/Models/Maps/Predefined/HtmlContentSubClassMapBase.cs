using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps.Predefined
{
    public abstract class HtmlContentSubClassMapBase<THtmlContent> : EntitySubClassMapBase<THtmlContent> where THtmlContent : IHtmlContent
    {
        protected HtmlContentSubClassMapBase(string moduleName, string tableName) : base(moduleName)
        {
            Table(tableName);

            Map(x => x.ActivationDate).Not.Nullable();
            Map(x => x.ExpirationDate).Nullable();
            Map(x => x.Html).Not.Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.CustomCss).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomCss).Not.Nullable();
            Map(x => x.CustomJs).Nullable().Length(int.MaxValue).LazyLoad();
            Map(x => x.UseCustomJs).Not.Nullable(); 
        }
    }
}
