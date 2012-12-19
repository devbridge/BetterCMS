using System;
using System.Text;
using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Projections;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Helper class for rendering layout section (region) content
    /// </summary>
    public class RegionContentWrapper : IDisposable
    {
        public const string PageContentIdPattern = "bcms-content-{0}";

        private readonly StringBuilder sb;
        private readonly PageContentProjection content;
        private readonly bool allowContentManagement;

        private HtmlHelper html;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionContentWrapper" /> class.
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="content">The region content.</param>
        /// <param name="allowContentManagement">if set to <c>true</c> allows content management.</param>
        public RegionContentWrapper(StringBuilder sb, HtmlHelper html, PageContentProjection content, bool allowContentManagement)
        {
            this.html = html;
            this.sb = sb;
            this.content = content;
            this.allowContentManagement = allowContentManagement;

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
            string cssClass = content.GetRegionWrapperCssClass(html);

            sb.AppendFormat(@"<div class=""bcms-content {0}"" data-page-content-id=""{1}"" data-content-id=""{2}"" data-page-content-version=""{3}"" data-content-version=""{4}"">", 
                cssClass,                
                content.PageContentId,
                content.ContentId,
                content.PageContentVersion,
                content.ContentVersion);
            sb.AppendLine();
        }

        /// <summary>
        /// Renders the closing tags.
        /// </summary>
        private void RenderClosingTags()
        {
            sb.AppendLine(@"</div>");
        }
    }
}