using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps.Predefined
{
    public abstract class PageContentEntityMapBase<TPageContent> : EntityMapBase<TPageContent> where TPageContent : IPageContent
    {
        protected PageContentEntityMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Order, "[Order]").Not.Nullable();           
        }
    }
}
