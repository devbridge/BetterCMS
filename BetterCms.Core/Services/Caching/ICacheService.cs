using System;

namespace BetterCms.Core.Services.Caching
{
    /// <summary>
    /// Defines the contract to manage cache items.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Sets object in cache with a specified key for specific time.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="obj">The cache object to set.</param>
        /// <param name="expiration">The cache item expiration.</param>
        void Set<T>(string key, T obj, TimeSpan expiration);

        /// <summary>
        /// Gets object from cache by specified key.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>Object from cache or default value of type T.</returns>
        T Get<T>(string key);

        /// <summary>
        /// Gets object from cache by a specified key with defined function to retrieve object.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="getCacheObject">A function to create cache object.</param>
        /// <returns>Object from cache or getCacheObject function value.</returns>
        T Get<T>(string key, TimeSpan expiration, Func<T> getCacheObject);

        /// <summary>
        /// Removes object from cache by a specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        void Remove(string key);
    }
}
