using System;

using BetterCms.Core.Models;

namespace BetterCms.Module.Users
{
    public class UsersModuleConstants
    {
        public const int UserNameMaxLength = MaxLength.Name + MaxLength.Name + 1;

        public static Guid LoginWidgetId = new Guid("DE0E47B2-728D-4BE6-904D-ED99CDDEDA4A");

        public const string PasswordRegularExpression = @"^.{4,255}$";
    }
}
