using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ChildContentOptionTranslationMap : EntityMapBase<ChildContentOptionTranslation>
    {
        public ChildContentOptionTranslationMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("ChildContentOptionTranslations");

            Map(x => x.Value).Nullable();

            References(x => x.ChildContentOption).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Language).Cascade.SaveUpdate();
        }
    }
}