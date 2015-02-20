using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Rendering;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.GetMainJsData
{
    public class GetMainJsDataCommand : CommandBase, ICommandOut<RenderMainJsViewModel>
    {
        /// <summary>
        /// The rendering service.
        /// </summary>
        private readonly IRenderingService renderingService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMainJsDataCommand" /> class.
        /// </summary>
        /// <param name="renderingService">The rendering service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        public GetMainJsDataCommand(IRenderingService renderingService, ICmsConfiguration cmsConfiguration)
        {
            this.renderingService = renderingService;
            this.cmsConfiguration = cmsConfiguration;
        }

        public RenderMainJsViewModel Execute()
        {
            RenderMainJsViewModel model = new RenderMainJsViewModel();
            model.JavaScriptModules = renderingService.GetJavaScriptIncludes();
            model.Version = cmsConfiguration.Version;
            model.UseMinReferences = cmsConfiguration.UseMinifiedResources;

#if (DEBUG)
            model.IsDebugMode = true;
#endif
            return model;
        }
    }
}