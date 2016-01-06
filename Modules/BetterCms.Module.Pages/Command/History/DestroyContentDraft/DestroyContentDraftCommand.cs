using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.History.DestroyContentDraft
{
    public class DestroyContentDraftCommand : CommandBase, ICommand<DestroyContentDraftCommandRequest, DestroyContentDraftCommandResponse>
    {
        private IDraftService draftService;
        
        private IWidgetService widgetService;

        public DestroyContentDraftCommand(IDraftService draftService, IWidgetService widgetService)
        {
            this.draftService = draftService;
            this.widgetService = widgetService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="ConcurrentDataException"></exception>
        /// <exception cref="CmsException"></exception>
        public DestroyContentDraftCommandResponse Execute(DestroyContentDraftCommandRequest request)
        {
            var content = draftService.DestroyDraftContent(request.Id, request.Version, Context.Principal);

            var response = new DestroyContentDraftCommandResponse
                       {
                           PublishedId = content.Original.Id,
                           Id = content.Original.Id,
                           OriginalId = content.Original.Id,
                           Version = content.Original.Version,
                           OriginalVersion = content.Original.Version,
                           WidgetName = content.Original.Name,
                           IsPublished = true,
                           HasDraft = false,
                           DesirableStatus = ContentStatus.Published
                       };

            // Try to cast to widget
            // TODO Widget categories
//            var widget = content.Original as HtmlContentWidget;
//            if (widget != null && widget.Category != null && !widget.Category.IsDeleted)
//            {
//                response.CategoryName = widget.Category.Name;
//            }

            if (request.IncludeChildRegions)
            {
                response.Regions = widgetService.GetWidgetChildRegionViewModels(content.Original);
            }

            return response;
        }
    }
}