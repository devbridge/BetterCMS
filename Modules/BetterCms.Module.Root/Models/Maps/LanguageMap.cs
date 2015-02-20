using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class LanguageMap : EntityMapBase<Language>
    {
        public LanguageMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Languages");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Code).Length(MaxLength.Name).Not.Nullable();
        }
    }
}
