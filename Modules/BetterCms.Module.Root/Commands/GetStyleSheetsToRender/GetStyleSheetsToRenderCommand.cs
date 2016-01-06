using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.GetStyleSheetsToRender
{
    public class GetStyleSheetsToRenderCommand : CommandBase, ICommand<GetStyleSheetsToRenderRequest, RenderStyleSheetIncludesViewModel>
    {
        /// <summary>
        /// The rendering service.
        /// </summary>
        private readonly IRenderingService renderingService;

        public GetStyleSheetsToRenderCommand(IRenderingService renderingService)
        {
            this.renderingService = renderingService;
        }

        public RenderStyleSheetIncludesViewModel Execute(GetStyleSheetsToRenderRequest request)
        {
            RenderStyleSheetIncludesViewModel model = new RenderStyleSheetIncludesViewModel
            {
                StyleSheetFiles = renderingService.GetStyleSheetIncludes(request.RenderPrivateCssIncludes, request.RenderPublicCssIncludes, request.ModuleDescriptorType)
            };

            return model;
        }
    }
}