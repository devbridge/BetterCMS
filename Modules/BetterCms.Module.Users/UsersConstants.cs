namespace BetterCms.Module.Users
{
    public class UsersConstants
    {
        public const string EmailRegularExpression = @"^[a-zA-Z0-9_\+-]+(\.[a-zA-Z0-9_\+-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.([a-zA-Z]{2,4})$";

        public const string PasswordRegularExpression = @"^.{4}(.{255})?$";
    }
}
