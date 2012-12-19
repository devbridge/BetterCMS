using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Widget.DeleteWidget
{
    /// <summary>
    /// Deletes widget.
    /// </summary>
    public class DeleteWidgetCommand : CommandBase, ICommand<DeleteWidgetRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(DeleteWidgetRequest request)
        {
            Repository.Delete<Root.Models.Widget>(request.WidgetId, request.Version);
            UnitOfWork.Commit();
            return true;
        }
    }
}