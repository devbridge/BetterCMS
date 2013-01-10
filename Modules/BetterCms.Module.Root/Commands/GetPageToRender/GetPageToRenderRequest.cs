using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Root.Commands.GetPageToRender
{
    public class GetPageToRenderRequest
    {
        public string VirtualPath { get; set; }

        public Guid? PreviewPageContentId { get; set; }

        public GetPageToRenderRequest()
        {
        }

        public GetPageToRenderRequest(string virtualPath)
        {
            VirtualPath = virtualPath;
        }

        public GetPageToRenderRequest(string virtualPath, Guid? previewPageContentId)
        {
            VirtualPath = virtualPath;
            PreviewPageContentId = previewPageContentId;
        }
    }
}