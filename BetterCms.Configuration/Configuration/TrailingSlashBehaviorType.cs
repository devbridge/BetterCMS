namespace BetterCms.Configuration
{
    /// <summary>
    /// URL end type.
    /// </summary>
    public enum TrailingSlashBehaviorType
    {
        /// <summary>
        /// Forces all URLs to end with slash.
        /// </summary>
        TrailingSlash = 0,

        /// <summary>
        /// Forces all URLs to end without slash.
        /// </summary>
        NoTrailingSlash = 1,

        /// <summary>
        /// Does not force URLs to end without or without slash.
        /// </summary>
        Mixed = 2
    }
}