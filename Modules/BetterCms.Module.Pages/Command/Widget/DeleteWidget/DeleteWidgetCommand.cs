using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.DeleteWidget
{
    /// <summary>
    /// Deletes widget.
    /// </summary>
    public class DeleteWidgetCommand : CommandBase, ICommand<DeleteWidgetRequest, bool>
    {
        private readonly IWidgetService widgetService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWidgetCommand" /> class.
        /// </summary>
        /// <param name="widgetService">The widget service.</param>
        public DeleteWidgetCommand(IWidgetService widgetService)
        {
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteWidgetRequest request)
        {
            return widgetService.DeleteWidget(request.WidgetId, request.Version);
        }
    }
}