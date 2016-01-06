using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterModules.Core.Web.Services.Caching;

namespace BetterCms.Sandbox.Mvc4.Custom
{
    public class CustomCacheTest : ICacheService
    {
        /// <summary>
        /// Sets object in cache with a specified key for specific time.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="obj">The cache object to set.</param>
        /// <param name="expiration">The cache item expiration.</param>
        public void Set<T>(string key, T obj, TimeSpan expiration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets object from cache by specified key.
        /// </summary>
        /// <typeparam name="T">Expected object type.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <returns>Object from cache or default value of type T.</returns>
        public T Get<T>(string key)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes object from cache by a specified key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        public void Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}