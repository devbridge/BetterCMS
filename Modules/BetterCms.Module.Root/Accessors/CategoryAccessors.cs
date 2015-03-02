using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Accessors;

namespace BetterCms.Module.Root.Accessors
{
    public static class CategoryAccessors
    {
        private static readonly IList<ICategoryAccessor> accessors = new List<ICategoryAccessor>();

        public static IList<ICategoryAccessor> Accessors
        {
            get
            {
                return accessors;
            }
        }

        public static void Register<T>(string name) 
            where T : class, IEntityCategory 
        {
            accessors.Add(new DefaultCategoryAccessor<T>(name));
        }

        public static void Register<T>()
            where T : class, ICategoryAccessor
        {
            var accessor = Activator.CreateInstance<T>();
            accessors.Add(accessor);
        }
    }
}