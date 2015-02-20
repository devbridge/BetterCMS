using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.GetProcessorJsData
{
    public class GetProcessorJsDataCommand : CommandBase, ICommandOut<RenderProcessorJsViewModel>
    {
        /// <summary>
        /// The rendering service.
        /// </summary>
        private readonly IRenderingService renderingService;

        public GetProcessorJsDataCommand(IRenderingService renderingService)
        {
            this.renderingService = renderingService;
        }

        public RenderProcessorJsViewModel Execute()
        {
            RenderProcessorJsViewModel model = new RenderProcessorJsViewModel();
            model.JavaScriptModules = renderingService.GetJavaScriptIncludes();

            return model;
        }
    }
}