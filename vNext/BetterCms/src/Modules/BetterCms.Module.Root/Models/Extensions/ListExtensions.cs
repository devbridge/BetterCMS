using System;
using System.Collections.Generic;

using BetterModules.Core.DataContracts;

namespace BetterCms.Module.Root.Models.Extensions
{
    public static class ListExtensions
    {
        public static void Add(this List<KeyValuePair<string, string>> list, string key, string value)
        {
             list.Add(new KeyValuePair<string, string>(key, value));
        }

        public static void RemoveDuplicates<T>(this IList<T> list, Comparison<T> comparison)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (comparison(list[i], list[j]) == 0)
                    {
                        list.RemoveAt(j);
                    }
                }
            }
        }  
     
        public static void RemoveDuplicateEntities<T>(this IList<T> list) where T : IEntity
        {
            list.RemoveDuplicates((a, b) => a.Id == b.Id ? 0 : -1);
        }
    }
}