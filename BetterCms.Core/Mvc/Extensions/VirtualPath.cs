using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BetterCms.Core.Mvc.Extensions
{
    public class VirtualPath
    {
        public static string Combine(params string[] parts)
        {
            return Path.Combine(parts).Replace(Path.DirectorySeparatorChar, '/');
        }

        public static bool IsLocalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            return path.Equals("local", StringComparison.OrdinalIgnoreCase) || path.Equals("(local)", StringComparison.OrdinalIgnoreCase);            
        }
    }
}
