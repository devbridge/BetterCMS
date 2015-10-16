using System;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderRequest
    {
        public Guid? PageId { get; set; }

        public string PageUrl { get; set; }

        public bool CanManageContent { get; set; }

        public bool HasContentAccess { get; set; }
        
        public bool IsAuthenticated { get; set; }

        public bool IsPreview { get; set; }

        public Guid? PreviewPageContentId { get; set; }

        public override string ToString()
        {
            return string.Format("PageId: {0}, PageUrl: {1}, CanManageContent: {2}, PreviewPageContentId: {3}", PageId, PageUrl, CanManageContent, PreviewPageContentId);
        }
    }
}