using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class ContentOptionMap : EntityMapBase<ContentOption>
    {
        public ContentOptionMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("ContentOptions");

            Map(x => x.Key, "[Key]").Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.DefaultValue).Length(MaxLength.Max).Nullable();
            Map(x => x.IsDeletable).Not.Nullable();

            References(x => x.Content).Cascade.SaveUpdate().LazyLoad();            
            References(x => x.CustomOption).Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.Translations).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
        }
    }
}