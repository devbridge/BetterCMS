// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionHelper.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root;

namespace BetterCms.Module.Pages.Helpers
{
    public static class RegionHelper
    {
        public const string DynamicRegionIdentifierPrefix = "ContentRegion";
        
        public const string WidgetDynamicRegionIdentifierPrefix = "WidgetRegion";

        public static int GetLastDynamicRegionNumber(IEnumerable<string> regionIdentifiers, string prefix = DynamicRegionIdentifierPrefix)
        {
            var maxNr = 0;
            var length = prefix.Length;
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
                        if (nrStr.Contains("_"))
                        {
                            nrStr = nrStr.Substring(0, nrStr.IndexOf("_", StringComparison.InvariantCultureIgnoreCase));
                        }
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