using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps.Predefined
{
    public abstract class PageContentOptionEntityMapBase<TPageContentOption> : EntityMapBase<TPageContentOption> where TPageContentOption : IPageContentOption
    {
        protected PageContentOptionEntityMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Value).Length(MaxLength.Text).Nullable().LazyLoad();         
        }
    }
}
