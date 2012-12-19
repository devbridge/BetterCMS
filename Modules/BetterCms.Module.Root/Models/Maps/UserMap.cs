using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models.Maps
{
    public class UserMap : EntityMapBase<User>
    {
        public UserMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Users");
            
            Map(x => x.UserName).Length(MaxLength.Name).Not.Nullable().Unique();
            Map(x => x.Email).Length(MaxLength.Email);
            Map(x => x.DisplayName).Length(MaxLength.Name);
        }
    }
}
