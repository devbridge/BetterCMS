using System;
using System.Collections.Generic;

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
    }
}