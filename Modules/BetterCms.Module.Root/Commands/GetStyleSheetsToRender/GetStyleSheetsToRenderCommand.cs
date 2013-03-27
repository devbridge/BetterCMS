using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

namespace BetterCms.Module.Root.Commands.GetStyleSheetsToRender
{
    public class GetStyleSheetsToRenderCommand : CommandBase, ICommandOut<RenderStyleSheetIncludesViewModel>
    {
        /// <summary>
        /// The rendering service.
        /// </summary>
        private readonly IRenderingService renderingService;

        public GetStyleSheetsToRenderCommand(IRenderingService renderingService)
        {
            this.renderingService = renderingService;
        }

        public RenderStyleSheetIncludesViewModel Execute()
        {
            RenderStyleSheetIncludesViewModel model = new RenderStyleSheetIncludesViewModel();
            model.StyleSheetFiles = renderingService.GetStyleSheetIncludes();

            return model;
        }
    }
}