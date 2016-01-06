using System;

using BetterCms.Core.Exceptions;

using Common.Logging;

using BetterModules.Core.Web.Services.Caching;

using Microsoft.ApplicationServer.Caching;

namespace BetterCms.Module.AppFabricCache
{
    /// <summary>
    /// Cache service implementation based on Microsoft.ApplicationServer.Caching.
    /// </summary>
    public class AppFabricCacheService : ICacheService
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Internal cache factory
        /// </summary>
        private static volatile DataCacheFactory internalCacheFactory;

        /// <summary>
        /// Internal data cache
        /// </summary>
        private static volatile DataCache internalCache;

        /// <summary>
        /// Object to lock a thread context.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The cache name.
        /// </summary>
        private readonly string cacheName;

        /// <summary>
        /// Indicates if service are disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppFabricCacheService" /> class.
        /// </summary>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public AppFabricCacheService(ICmsConfiguration cmsConfiguration)
        {            
            var cacheConfiguration = cmsConfiguration.Cache;
            cacheName = cacheConfiguration.GetValue("cacheName");
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="AppFabricCacheService" /> class.
        /// </summary>
        ~AppFabricCacheService()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the cache factory.
        /// </summary>
        private DataCacheFactory CacheFactory
        {
            get
            {
                try
                {
                   if (internalCacheFactory == null)
                   {
                       lock (lockObject)
                       {
                           if (internalCacheFactory == null)
                           {
                               internalCacheFactory = new DataCacheFactory();
                               if (Log.IsTraceEnabled)
                               {
                                   Log.Trace("AppFabric cache factory created.");
                               }
                           }
                       }
                   }

                    return internalCacheFactory;
                }
                catch (Exception ex)
                {
                    lock (lockObject)
                    {
                        internalCacheFactory = null;
                        internalCache = null;
                    }

                    throw new CmsException("Failed to create distributed caching factory object.", ex);
                }
            }
        }

        /// <summary>
        /// Gets the cache.
        /// </summary>
        private DataCache Cache
        {
            get
            {
                try
                {
                    if (CacheFactory != null && internalCache == null)
                    {
                        lock (lockObject)
                        {
                            if (internalCache == null)
                            {
                                internalCache = CacheFactory.GetCache(cacheName);
                                if (Log.IsTraceEnabled)
                                {
                                    Log.TraceFormat("AppFabric cache {0} access object created.", cacheName);
                                }
                            }
                        }
                    }

                    return internalCache;
                }
                catch (Exception ex)
                {
                    lock (lockObject)
                    {
                        internalCache = null;
                    }

                    string message = string.Format("Failed to create distributed cache {0} object.", cacheName);
                    throw new CmsException(message, ex);
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Adds object to cache with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of the object.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="obj">The object.</param>
        /// <param name="expiration">The cache expiration.</param>
        public void Set<T>(string key, T obj, TimeSpan expiration)
        {
            try
            {
                if (obj == null)
                {
                    Remove(key);
                }
                else
                {
                    key = key.ToUpperInvariant();
                    Cache.Put(key, obj, expiration);
                }
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to put value to cache with key={0}. {1}", key, ex.Message);
                throw new CmsException(message, ex);
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>Object from cache</returns>
        public T Get<T>(string key)
        {
            return Get<T>(key, TimeSpan.MinValue, null);
        }
 
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="expiration">The expiration.</param>
        /// <param name="getCacheObjectDelegate">The get cache object delegate.</param>
        /// <returns>Object from cache</returns>
        public T Get<T>(string key, TimeSpan expiration, Func<T> getCacheObjectDelegate)
        {
            bool updateRequired = false;
            T obj = default(T);
            try
            {                
                object objectFromCache = Cache.Get(key.ToUpperInvariant());
                if (objectFromCache != null && objectFromCache.GetType() == typeof(T))
                {                    
                    try
                    {
                        obj = (T)objectFromCache;
                    }
                    catch
                    {
                        // If cast is not successful then need to update it in cache with new object.
                        updateRequired = true;
                    }
                }
                else
                {
                    updateRequired = true;
                }

                if (updateRequired && getCacheObjectDelegate != null)
                {
                    obj = getCacheObjectDelegate();
                    Set(key, obj, expiration);
                }

                return obj;
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to get value from cache with key={0}. {1}", key, ex.Message);
                throw new CmsException(message, ex);
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            try
            {
                Cache.Remove(key.ToUpperInvariant());
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to remove value from cache with key={0}. {1}", key, ex.Message);
                throw new CmsException(message, ex);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    lock (lockObject)
                    {
                        if (internalCacheFactory != null)
                        {
                            internalCacheFactory.Dispose();
                        }

                        internalCacheFactory = null;
                        internalCache = null;
                    }
                }

                disposed = true;
            }
        }
    }
}
