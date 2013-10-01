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

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionContentWrapper" /> class.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="content">The region content.</param>
        /// <param name="allowContentManagement">if set to <c>true</c> allows content management.</param>
        public RegionContentWrapper(StringBuilder sb, PageContentProjection content, bool allowContentManagement)
        {
            this.sb = sb;
            this.content = content;
            this.allowContentManagement = allowContentManagement;

            clearFixClassName = CmsContext.Config.ContentEndingDivCssClassName;
            renderClearFixDiv = CmsContext.Config.RenderContentEndingDiv;

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
                    @"<div class=""{0}"" data-page-content-id=""{1}"" data-content-id=""{2}"" data-page-content-version=""{3}"" data-content-version=""{4}"" data-content-type=""{5}"" data-content-title=""{6}"" {7}></div>",
                    ContentStartClassName, // 0
                    content.PageContentId, // 1
                    content.ContentId, // 2
                    content.PageContentVersion, // 3
                    content.ContentVersion, // 4
                    content.GetContentWrapperType(), // 5
                    content.GetTitle(), // 6
                    content.PageContentStatus == ContentStatus.Draft ? " data-draft=\"true\"" : null); // 7
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