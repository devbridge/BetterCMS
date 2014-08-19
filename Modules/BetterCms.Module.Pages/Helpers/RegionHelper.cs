using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root;

namespace BetterCms.Module.Pages.Helpers
{
    public static class RegionHelper
    {
        public const string DynamicRegionIdentifierPrefix = "ContentRegion";

        public static int GetLastDynamicRegionNumber(IEnumerable<string> regionIdentifiers)
        {
            var maxNr = 0;
            var length = DynamicRegionIdentifierPrefix.Length;
            foreach (var identifier in regionIdentifiers.Distinct())
            {
                int nr = 0;
                if (String.Equals(identifier, RootModuleConstants.MainContentRegionIdentifier, StringComparison.InvariantCultureIgnoreCase))
                {
                    nr = 1;
                }
                else
                {
                    try
                    {
                        var nrStr = identifier.Substring(length);
                        Int32.TryParse(nrStr, out nr);
                    }
                    catch
                    {
                        // Nothing to do: just catch the exception
                    }
                }

                if (nr > maxNr)
                {
                    maxNr = nr;
                }
            }

            return maxNr;
        }
    }
}