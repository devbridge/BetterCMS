using System;
using System.Text;
using System.Web.Mvc;

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
            string id = string.Format(PageContentIdPattern, content.ContentId);

            if (allowContentManagement)
            {
                string cssClass = content.GetRegionWrapperCssClass(html);

                sb.AppendFormat(@"<div id=""{0}"" class=""bcms-content {1}"" data-page-content-id=""{2}"" data-content-id=""{3}"" data-page-content-version=""{4}"" data-content-version=""{5}"">",
                    id,
                    cssClass,
                    content.PageContentId,
                    content.ContentId,
                    content.PageContentVersion,
                    content.ContentVersion);
                sb.AppendLine();
            }
            else
            {
                sb.AppendFormat(@"<div id=""{0}"">", id);
                sb.AppendLine();
            }
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