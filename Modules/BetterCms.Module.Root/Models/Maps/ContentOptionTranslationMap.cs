using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentOptionTranslationMap : EntityMapBase<ContentOptionTranslation>
    {
        public ContentOptionTranslationMap() : base(RootModuleDescriptor.ModuleName)
        {
            Table("ContentOptionTranslations");

            Map(x => x.Value).Nullable();

            References(x => x.ContentOption).Cascade.SaveUpdate().LazyLoad();
            References(x => x.Language).Cascade.SaveUpdate();
        }
    }
}