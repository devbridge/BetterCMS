using BetterModules.Core.Models;

namespace BetterCms.Module.Users
{
    public class UsersModuleConstants
    {
        public const int UserNameMaxLength = MaxLength.Name + MaxLength.Name + 1;        

        public const string PasswordRegularExpression = @"^\S.{2,255}\S$";
    }
}
