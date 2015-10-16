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