using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ModuleContentVersionMap : EntityMapBase<ModuleContentVersion>
    {
        public ModuleContentVersionMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("ModulesContentVersions");

            Map(x => x.ModuleName).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.ContentVersion).Not.Nullable();
        }
    }
}
