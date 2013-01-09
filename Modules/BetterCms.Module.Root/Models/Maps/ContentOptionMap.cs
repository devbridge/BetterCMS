using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentOptionMap : ContentOptionEntityMapBase<ContentOption>
    {
        public ContentOptionMap()
            : base(RootModuleDescriptor.ModuleName, "ContentOptions")
        {
            References(x => x.Content).Cascade.SaveUpdate().LazyLoad();
        }
    }
}