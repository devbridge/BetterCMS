namespace BetterCms.Core.Security
{
    /// <summary>
    /// Object access level.
    /// </summary>
    public enum AccessLevel
    {
        NoPermissions = 0,
        Deny = 1,
        Read = 2,
        ReadWrite = 3
    }
}