namespace BetterCms.Configuration
{
    /// <summary>
    /// BetterCMS cache service types.
    /// </summary>
    public enum CacheServiceType
    {
        /// <summary>
        /// Default cache service based on the server HttpRuntime cache. This is default.
        /// </summary>
        HttpRuntime = 0,

        /// <summary>
        /// Cache service is automatically picked by scanning installed modules for the caching service.
        /// </summary>
        Auto = 1,

        /// <summary>
        /// Custom storage provider
        /// </summary>
        Custom = 2
    }
}