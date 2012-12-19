using System;
using System.Web;
using System.Web.Caching;

using Common.Logging;

namespace BetterCms.Core.Services.Caching
{
    /// <summary>
    /// Provides a cache service implementation based on the HttpRuntime cache.
    /// </summary>
    public class HttpRuntimeCacheService : ICacheService
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Sets object in cache with a specified key for specific time.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="obj">The cache object to set.</param>
        /// <param name="expiration">The cache item expiration.</param>
        public void Set<T>(string key, T obj, TimeSpan expiration)
        {
            Set(key, obj, expiration, null);
        }

        /// <summary>
        /// Gets object from cache by specified key.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>Object from cache or default value of type T.</returns>
        public T Get<T>(string key)
        {            
            object obj;

            try
            {
                obj = HttpRuntime.Cache[key.ToUpperInvariant()];
            }
            catch (Exception ex)
            {
                Logger.WarnFormat("Failed to retrieve cache item {0}.", ex, key);
                obj = null;                
            }
            
            if (obj == null)
            {
                return default(T);
            }

            T converted;
            try
            {
                converted = (T)obj;
            }
            catch (Exception ex)
            {
                Logger.WarnFormat("Failed to convert cache item {0} of type {1} to type {2}.", ex, key, obj.GetType().FullName, typeof(T).FullName);
                converted = default(T);
            }

            return converted;
        }
        
        /// <summary>
        /// Gets object from cache by a specified key with defined function to retrieve object.
        /// </summary>
        /// <typeparam name="T">Expected type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="getCacheObject">A function to create cache object.</param>
        /// <returns>Object from cache or getCacheObject function value.</returns>
        public T Get<T>(string key, TimeSpan expiration, Func<T> getCacheObject)
        {
            return Get(key, expiration, getCacheObject, null);
        }        

        /// <summary>
        /// Removes object from cache by a specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Remove(string key)
        {
            if (HttpRuntime.Cache[key.ToUpperInvariant()] != null)
            {
                HttpRuntime.Cache.Remove(key.ToUpperInvariant());
            }
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="cacheItemRemovedCallback">The cache item removed callback.</param>
        internal void Set<T>(string key, T obj, TimeSpan expiration, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            if (obj == null)
            {
                Remove(key);
            }
            else
            {
                HttpRuntime.Cache.Add(
                    key.ToUpperInvariant(), 
                    obj, 
                    null, 
                    DateTime.UtcNow.Add(expiration),
                    Cache.NoSlidingExpiration, 
                    CacheItemPriority.NotRemovable,
                    cacheItemRemovedCallback);
            }
        }

        /// <summary>
        /// Gets an item from cache by specified key.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="expiration">The cache item expiration.</param>
        /// <param name="getCacheObject">Function to create cache item.</param>
        /// <param name="cacheItemRemovedCallback">The cache item removed callback.</param>
        /// <returns>Object from cache or getCacheObject function value.</returns>
        internal T Get<T>(string key, TimeSpan expiration, Func<T> getCacheObject, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            T obj = Get<T>(key);

            if (obj == null)
            {
                obj = getCacheObject();
                Set(key, obj, expiration, cacheItemRemovedCallback);
            }

            return obj;
        }
    }
}
