using System;

namespace BetterCms.Module.Users
{
    public class UsersModuleConstants
    {
        public static Guid LoginWidgetId = new Guid("DE0E47B2-728D-4BE6-904D-ED99CDDEDA4A");

        public const string PasswordRegularExpression = @"^.{4}(.{255})?$";
    }
}
