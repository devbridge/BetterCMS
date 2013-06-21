using System.Collections.Generic;

namespace BetterCms.Module.MediaManager.Models.Extensions
{
    public static class ListExtensions
    {
        public static void Add(this List<KeyValuePair<string, string>> list, string key, string value)
        {
             list.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}