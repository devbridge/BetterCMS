using BetterCms.Core.Models;

namespace BetterCms.Module.AccessControl.Models.Maps
{
    public class UserAccessMap : EntityMapBase<UserAccess>
    {
        public UserAccessMap()
            : base(UserAccessModuleDescriptor.ModuleName)
        {
            Table("UserAccess");

            Map(x => x.ObjectId).Not.Nullable();
            Map(x => x.RoleOrUser).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();
        }
    }
}