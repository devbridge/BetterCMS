using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps.Predefined
{
    public abstract class ContentEntityMapBase<TContent> : EntityMapBase<TContent> where TContent : IContent
    {
        protected ContentEntityMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.PreviewUrl).Length(MaxLength.Url).Nullable();            
        }
    }
}
