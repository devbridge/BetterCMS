
using BetterModules.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class UserMap : EntityMapBase<User>
    {
        public UserMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Users");

            Map(x => x.UserName).Length(UsersModuleConstants.UserNameMaxLength).Not.Nullable();
            Map(x => x.FirstName).Length(MaxLength.Name).Nullable();
            Map(x => x.LastName).Length(MaxLength.Name).Nullable();
            Map(x => x.Password).Length(MaxLength.Password).Not.Nullable();
            Map(x => x.Email).Length(MaxLength.Email).Not.Nullable();
            Map(x => x.Salt).Length(MaxLength.Password).Not.Nullable();
            
            References(x => x.Image).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.UserRoles).KeyColumn("UserId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}