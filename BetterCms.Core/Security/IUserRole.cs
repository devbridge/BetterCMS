namespace BetterCms.Core.Security
{
    /// <summary>
    /// Defines interface to access entity properties.
    /// </summary>
    public interface IUserRole
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the localized name.
        /// </summary>
        string LocalizedName { get; }
    }
}
