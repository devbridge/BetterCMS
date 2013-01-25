using System;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderRequest
    {
        public Guid? PageId { get; set; }

        public string PageUrl { get; set; }

        public bool CanManageContent { get; set; }

        public Guid? PreviewPageContentId { get; set; }
    }
}