// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpHelper.cs" company="Devbridge Group LLC">
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
namespace BetterCms.Module.Pages.Helpers
{
    /// <summary>
    /// Helper class for http related tasks.
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// Virtual the path exists.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns>Returns true if file exists or false otherwise.</returns>
        public static bool VirtualPathExists(string virtualPath)
        {
            if (!string.IsNullOrWhiteSpace(virtualPath))
            {
                // Fix virtual path
                if (!virtualPath.StartsWith("~", System.StringComparison.Ordinal))
                {
                    if (virtualPath.StartsWith("/", System.StringComparison.Ordinal))
                    {
                        virtualPath = string.Concat("~", virtualPath);
                    }
                    else
                    {
                        virtualPath = string.Concat("~/", virtualPath);
                    }
                }

                return System.Web.Hosting.HostingEnvironment.VirtualPathProvider.FileExists(virtualPath);
            }
            
            return false;
        }
    }
}