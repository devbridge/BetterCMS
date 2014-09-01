using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class SettingMap: EntityMapBase<Setting>
    {
        public SettingMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Setting");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Value).Length(MaxLength.Name).Nullable();
            Map(x => x.ModuleId).Not.Nullable();
        }
    }
}