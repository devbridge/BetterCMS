using BetterCms.Core.Models;

namespace BetterCms.Module.AccessControl.Models.Maps
{
    public class UserAccessMap : EntitySubClassMapBase<UserAccess>
    {
        public UserAccessMap()
            : base(UserAccessModuleDescriptor.ModuleName)
        {
            Table("UserAccess");

            Map(x => x.ObjectId).Not.Nullable();
            Map(x => x.User).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.AccessLevel).Not.Nullable();
        }
    }
}