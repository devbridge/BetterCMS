using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps.Predefined
{
    public abstract class ContentOptionEntityMapBase<TContentOption> : EntityMapBase<TContentOption> where TContentOption : IContentOption
    {
        protected ContentOptionEntityMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.DefaultValue).Length(MaxLength.Max).Nullable().LazyLoad();
        }
    }
}
