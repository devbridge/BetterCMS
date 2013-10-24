using System.Linq;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Root.Models;
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
            UnitOfWork.BeginTransaction();

            var widget = Repository.First<Root.Models.Widget>(request.WidgetId);
            widget.Version = request.Version;

            var isWidgetInUse = Repository.AsQueryable<PageContent>().Any(f => f.Content.Id == request.WidgetId && !f.IsDeleted && !f.Page.IsDeleted);

            if (isWidgetInUse)
            {
                throw new ValidationException(() => string.Format(PagesGlobalization.Widgets_CanNotDeleteWidgetIsInUse_Message, widget.Name), 
                                              string.Format("A widget {0}(id={1}) can't be deleted because it is in use.", widget.Name, request.WidgetId));
            }

            Repository.Delete(widget);

            if (widget.ContentOptions != null)
            {
                foreach (var option in widget.ContentOptions)
                {
                    Repository.Delete(option);
                }
            }

            UnitOfWork.Commit();

            // Notify.
            Events.PageEvents.Instance.OnWidgetDeleted(widget);

            return true;
        }
    }
}