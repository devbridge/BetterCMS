using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.PreviewWidget
{
    /// <summary>
    /// Command for previewing widget
    /// </summary>
    public class PreviewWidgetCommand : CommandBase, ICommand<PreviewWidgetCommandRequest, RenderPageViewModel>
    {
        /// <summary>
        /// The preview service
        /// </summary>
        private readonly IPreviewService previewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewWidgetCommand" /> class.
        /// </summary>
        /// <param name="previewService">The preview service.</param>
        public PreviewWidgetCommand(IPreviewService previewService)
        {
            this.previewService = previewService;
        }

        /// <summary>
        /// Executes the specified widget id.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Rendered page view model</returns>
        public RenderPageViewModel Execute(PreviewWidgetCommandRequest request)
        {
            return previewService.GetContentPreviewViewModel(request.WidgetId, Context.Principal, request.IsJavaScriptEnabled);
        }
    }
}