using BetterModules.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class RoleMap : EntityMapBase<Role>
    {
        public RoleMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Roles");

            Map(x => x.Name).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Description).Length(MaxLength.Name).Nullable();
            Map(x => x.IsSystematic).Not.Nullable();

            HasMany(x => x.UserRoles).KeyColumn("RoleId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}