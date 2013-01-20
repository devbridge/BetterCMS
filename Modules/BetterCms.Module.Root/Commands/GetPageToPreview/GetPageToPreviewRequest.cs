using System;

namespace BetterCms.Module.Root.Commands.GetPageToPreview
{
    public class GetPageToPreviewRequest
    {
        public Guid PageId { get; set; }

        public Guid PageContentId { get; set; }
    }
}