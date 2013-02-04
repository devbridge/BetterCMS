
using BetterCms.Core.Models;

namespace BetterCms.Module.Users.Models.Maps
{
    public class UsersMap : EntityMapBase<Users>
    {
        public UsersMap()
            : base(UsersModuleDescriptor.ModuleName)
        {
            Table("Users");

            Map(x => x.UserName).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.FirstName).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.LastName).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Password).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Email).Length(MaxLength.Email).Not.Nullable();
            References(x => x.Image).Cascade.SaveUpdate().LazyLoad();
            //References(x => x.ImageId).Cascade.SaveUpdate().LazyLoad().Nullable();
            //Map(x => x.ImageId).Length()
        }
    }
}