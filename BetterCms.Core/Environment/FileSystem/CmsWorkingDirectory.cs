// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsWorkingDirectory.cs" company="Devbridge Group LLC">
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
using System.Web.Hosting;

using BetterModules.Core.Environment.FileSystem;

namespace BetterCms.Core.Environment.FileSystem
{
    public class CmsWorkingDirectory : DefaultWorkingDirectory
    {
        private readonly ICmsConfiguration configuration;

        public CmsWorkingDirectory(ICmsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public override string GetWorkingDirectoryPath()
        {
            if (!string.IsNullOrWhiteSpace(configuration.WorkingDirectoryRootPath))
            {
                return HostingEnvironment.MapPath(configuration.WorkingDirectoryRootPath);
            }

            return "~/App_Data/BetterCMS";
        }
    }
}
