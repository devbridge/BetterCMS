using BetterCms.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps.Predefined
{
    public abstract class ServerControlWidgetSubClassMapBase<TServerControlWidget> : EntitySubClassMapBase<TServerControlWidget> where TServerControlWidget : IServerControlWidget
    {
        protected ServerControlWidgetSubClassMapBase(string moduleName, string tableName)
            : base(moduleName)
        {
            Table(tableName);

            Map(x => x.Url).Not.Nullable();
        }
    }
}
