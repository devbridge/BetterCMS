using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public abstract class SaveWidgetCommandBase<TWidget> : CommandBase, ICommand<SaveWidgetCommandRequest<TWidget>, SaveWidgetResponse> 
        where TWidget : WidgetViewModel
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The widget to save.</param>
        public abstract SaveWidgetResponse Execute(SaveWidgetCommandRequest<TWidget> request);
    }
}