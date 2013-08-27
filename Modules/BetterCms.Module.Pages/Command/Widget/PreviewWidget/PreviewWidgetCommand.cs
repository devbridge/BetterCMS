using System;

using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Pages.Command.Widget.PreviewWidget
{
    /// <summary>
    /// Command for previewing widget
    /// </summary>
    public class PreviewWidgetCommand : CommandBase, ICommand<Guid, RenderPageViewModel>
    {
        /// <summary>
        /// The preview service
        /// </summary>
        private IPreviewService previewService;

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
        /// <param name="widgetId">The widget id.</param>
        /// <returns></returns>
        public RenderPageViewModel Execute(Guid widgetId)
        {
            return previewService.GetContentPreviewViewModel(widgetId, Context.User);
        }
    }
}