using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ModuleMap : EntityMapBase<Module>
    {
        public ModuleMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("Modules");

            Map(x => x.Name).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Description).Not.Nullable().Length(MaxLength.Text);
            Map(x => x.ModuleVersion).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Enabled).Not.Nullable();            
        }
    }
}
