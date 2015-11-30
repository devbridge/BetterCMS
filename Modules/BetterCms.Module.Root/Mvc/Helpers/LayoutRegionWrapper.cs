// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LayoutRegionWrapper.cs" company="Devbridge Group LLC">
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
using System.Text;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Helper class for rendering layout section (region)
    /// </summary>
    public class LayoutRegionWrapper : IDisposable
    {
        private const string RegionStartClassName = "bcms-region-start";
        private const string RegionEndClassName = "bcms-region-end";

        private readonly StringBuilder sb;
        private readonly PageRegionViewModel region;
        private readonly bool allowContentManagement;
        private readonly bool isInvisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRegionWrapper" /> class.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="region">The region.</param>
        /// <param name="allowContentManagement">if set to <c>true</c> allows content management.</param>
        /// <param name="isInvisible">if set to <c>true</c> [is invisible].</param>
        public LayoutRegionWrapper(StringBuilder sb, PageRegionViewModel region, bool allowContentManagement, bool isInvisible = false)
        {
            this.sb = sb;
            this.region = region;
            this.allowContentManagement = allowContentManagement;
            this.isInvisible = isInvisible;

            if (allowContentManagement)
            {
                RenderOpeningTags();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (allowContentManagement)
            {
                RenderClosingTags();
            }
        }

        /// <summary>
        /// Renders the opening tags.
        /// </summary>
        private void RenderOpeningTags()
        {
            sb.AppendFormat(@"<div class=""{0}"" data-id=""{1}"" data-identifier=""{2}""{3}></div>", 
                RegionStartClassName,               // 0 
                region.RegionId,                    // 1
                region.RegionIdentifier,            // 2
                isInvisible ? " data-invisible=\"true\"" : null); // 3
            sb.AppendLine();
        }

        /// <summary>
        /// Renders the closing tags.
        /// </summary>
        private void RenderClosingTags()
        {
            sb.AppendFormat(@"<div class=""{0}""></div>", RegionEndClassName).AppendLine();
        }
    }
}