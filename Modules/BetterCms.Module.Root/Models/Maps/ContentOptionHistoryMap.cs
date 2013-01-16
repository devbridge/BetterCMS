using BetterCms.Module.Root.Models.Maps.Predefined;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentOptionHistoryMap : ContentOptionEntityMapBase<ContentOptionHistory>
    {
        public ContentOptionHistoryMap()
            : base(RootModuleDescriptor.ModuleName, "ContentOptionHistory")
        {          
            References(x => x.ContentHistory).Cascade.SaveUpdate().LazyLoad();
        }
    }
}