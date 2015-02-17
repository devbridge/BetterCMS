using System;
using System.Web;
using System.Web.Caching;

using Devbridge.Platform.Core.Web.Services.Caching;

using NUnit.Framework;

namespace Devbridge.Platform.Core.Web.Tests.Services.Caching
{
    [TestFixture]
    public class HttpRuntimeCacheServiceTests : TestBase
    {
        [Test]
        public void ShouldSetItem_WithTimeSpan_Correctly()
        {
            var cachingService = new HttpRuntimeCacheService();
            var timespan = TimeSpan.FromSeconds(1);
            const string key = "TEST_CACHEKEY_2";
            ClearCache(key);
            var cacheObject = new CacheObject(key);

            cachingService.Set(key, cacheObject, timespan);
            Assert.AreEqual(HttpRuntime.Cache[key], cacheObject);

            System.Threading.Thread.Sleep(timespan);
            Assert.IsNull(HttpRuntime.Cache[key]);
        }

        [Test]
        public void ShouldGetItem_Correctly()
        {
            var cachingService = new HttpRuntimeCacheService();
            const string key = "TEST_CACHEKEY_3";
            ClearCache(key);
            var cacheObject = new CacheObject(key);

            HttpRuntime.Cache.Add(key, cacheObject, null, DateTime.Now.Add(TimeSpan.FromMinutes(1)), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            var cachedItem = cachingService.Get<CacheObject>(key);
            Assert.AreEqual(cachedItem, cacheObject);
        }

        [Test]
        public void ShouldGetItem_WithDelegate_WithExpiration_Correctly()
        {
            var timespan = TimeSpan.FromSeconds(3);
            var cachingService = new HttpRuntimeCacheService();
            const string key = "TEST_CACHEKEY_5";
            ClearCache(key);
            var cacheObject = new CacheObject(key);

            Assert.IsNull(HttpRuntime.Cache[key]);
            var cachedItem = cachingService.Get(key, timespan, () => cacheObject);
            Assert.AreEqual(cachedItem, cacheObject);
            Assert.AreEqual(HttpRuntime.Cache[key], cacheObject);

            System.Threading.Thread.Sleep(timespan);
            Assert.IsNull(HttpRuntime.Cache[key]);
        }

        [Test]
        public void ShouldRemoveItem_Successfully()
        {
            const string key = "TEST_CACHEKEY_6";
            var cacheObject = new CacheObject(key);

            HttpRuntime.Cache.Add(key, cacheObject, null, DateTime.Now.Add(TimeSpan.FromMinutes(1)), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            Assert.AreEqual(HttpRuntime.Cache[key], cacheObject);

            var cachingService = new HttpRuntimeCacheService();
            cachingService.Remove(key);
            Assert.IsNull(HttpRuntime.Cache[key]);
        }

        private void ClearCache(string key)
        {
            if (HttpRuntime.Cache[key] != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        private class CacheObject
        {
            public CacheObject(string key)
            {
                Key = key;
            }

            public string Key { get; set; }
        }
    }
}
