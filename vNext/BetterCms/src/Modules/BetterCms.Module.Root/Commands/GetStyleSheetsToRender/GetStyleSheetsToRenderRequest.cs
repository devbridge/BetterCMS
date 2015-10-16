using System;

namespace BetterCms.Module.Root.Commands.GetStyleSheetsToRender
{
    public class GetStyleSheetsToRenderRequest
    {
        public bool RenderPrivateCssIncludes { get; set; }

        public bool RenderPublicCssIncludes { get; set; }

        public Type ModuleDescriptorType { get; set; }
    }
}