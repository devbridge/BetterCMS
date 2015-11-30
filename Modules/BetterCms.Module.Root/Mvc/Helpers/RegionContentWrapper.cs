// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegionContentWrapper.cs" company="Devbridge Group LLC">
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

using BetterCms.Core;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Helper class for rendering layout section (region) content
    /// </summary>
    public class RegionContentWrapper : IDisposable
    {
        private const string ContentStartClassName = "bcms-content-start";
        private const string ContentEndClassName = "bcms-content-end";

        private readonly StringBuilder sb;
        private readonly PageContentProjection content;
        private readonly bool allowContentManagement;
        private readonly string clearFixClassName;
        private readonly bool renderClearFixDiv;
        private readonly bool isInvisible;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionContentWrapper" /> class.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="content">The region content.</param>
        /// <param name="allowContentManagement">if set to <c>true</c> allows content management.</param>
        /// <param name="isInvisible">if set to <c>true</c> [is invisible].</param>
        public RegionContentWrapper(StringBuilder sb, PageContentProjection content, bool allowContentManagement, bool isInvisible = false)
        {
            this.sb = sb;
            this.content = content;
            this.allowContentManagement = allowContentManagement;

            clearFixClassName = CmsContext.Config.ContentEndingDivCssClassName;
            renderClearFixDiv = CmsContext.Config.RenderContentEndingDiv;
            this.isInvisible = isInvisible;

            RenderOpeningTags();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            RenderClosingTags();
        }

        /// <summary>
        /// Renders the opening tags.
        /// </summary>
        private void RenderOpeningTags()
        {
            if (allowContentManagement)
            {
                sb.AppendFormat(
                    @"<div class=""{0}"" data-page-content-id=""{1}"" data-content-id=""{2}"" data-page-content-version=""{3}"" data-content-version=""{4}"" data-content-type=""{5}"" data-content-title=""{6}"" {7}{8}></div>",
                    ContentStartClassName, // 0
                    content.PageContentId, // 1
                    content.ContentId, // 2
                    content.PageContentVersion, // 3
                    content.ContentVersion, // 4
                    content.GetContentWrapperType(), // 5
                    content.GetTitle(), // 6
                    content.PageContentStatus == ContentStatus.Draft ? " data-draft=\"true\"" : null, // 7
                    isInvisible ? " data-invisible=\"true\"" : null // 8
                    );
                sb.AppendLine();
            }
        }

        /// <summary>
        /// Renders the closing tags.
        /// </summary>
        private void RenderClosingTags()
        {
            if (allowContentManagement)
            {
                sb.AppendFormat(@"<div class=""{0} bcms-clearfix {1}"" data-hide=""{2}""></div>", 
                    ContentEndClassName, 
                    clearFixClassName,
                    !renderClearFixDiv ? "true" : "false").AppendLine();
            }
            else
            {
                if (renderClearFixDiv)
                {
                    sb.AppendFormat(@"<div class=""bcms-clearfix {0}""></div>", clearFixClassName).AppendLine();
                }
            }
        }
    }
}