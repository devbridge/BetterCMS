// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomCacheTest.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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