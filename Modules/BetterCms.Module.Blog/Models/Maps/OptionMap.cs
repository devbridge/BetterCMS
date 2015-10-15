using BetterModules.Core.Models;

namespace BetterCms.Module.Blog.Models.Maps
{
    public class OptionMap : EntityMapBase<Option>
    {
        public OptionMap()
            : base(BlogModuleDescriptor.ModuleName)
        {
            Table("Options");

            Map(x => x.DefaultContentTextMode).Not.Nullable();

            References(x => x.DefaultLayout).Cascade.SaveUpdate().LazyLoad();
            References(x => x.DefaultMasterPage).Cascade.SaveUpdate().LazyLoad();
        }
    }
}