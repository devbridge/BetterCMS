using System.Linq;

using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Widgets;

using CategoryEntity = BetterCms.Module.Root.Models.Category;

namespace BetterCms.Module.Pages.Command.Widget.SaveWidget
{
    public class SaveServerControlWidgetCommand : SaveWidgetCommandBase<EditServerControlWidgetViewModel>
    {
        public virtual IWidgetService WidgetService { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public override SaveWidgetResponse Execute(SaveWidgetCommandRequest<EditServerControlWidgetViewModel> request)
        {
            var widget = WidgetService.SaveServerControlWidget(request.Content);

            return new SaveWidgetResponse
                       {
                           Id = widget.Id,
                           OriginalId = widget.Id,
                           WidgetName = widget.Name,
                           Version = widget.Version,
                           OriginalVersion = widget.Version,
                           WidgetType = WidgetType.ServerControl.ToString(),
                           IsPublished = widget.Status == ContentStatus.Published,
                           HasDraft = widget.Status == ContentStatus.Draft || widget.History != null && widget.History.Any(f => f.Status == ContentStatus.Draft),
                           DesirableStatus = request.Content.DesirableStatus,
                           PreviewOnPageContentId = request.Content.PreviewOnPageContentId
                       };
        }
    }
}