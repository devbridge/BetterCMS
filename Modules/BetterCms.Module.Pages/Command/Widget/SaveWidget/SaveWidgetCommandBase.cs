using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public abstract class SaveWidgetCommandBase<TWidget> : CommandBase, ICommand<TWidget, SaveWidgetResponse> where TWidget : WidgetViewModel
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The widget to save.</param>
        public abstract SaveWidgetResponse Execute(TWidget request);
    }
}