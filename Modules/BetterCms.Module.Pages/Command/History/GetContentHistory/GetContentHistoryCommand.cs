using System;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.History;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.History.GetContentHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetContentHistoryCommand : CommandBase, ICommand<GetContentHistoryRequest, ContentHistoryViewModel>
    {
        /// <summary>
        /// The history service
        /// </summary>
        private readonly IHistoryService historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentHistoryCommand" /> class.
        /// </summary>
        public GetContentHistoryCommand(IHistoryService historyService)
        {
            this.historyService = historyService;
        }

        private bool userHasPublishRole { get; set; }
        private bool userHasAdministrationRole { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The view model with list of history view models</returns>
        public ContentHistoryViewModel Execute(GetContentHistoryRequest request)
        {
            userHasPublishRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.PublishContent);
            userHasAdministrationRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration);

            var history = historyService.GetContentHistory(request.ContentId, request).Select(Convert).ToList();
            return new ContentHistoryViewModel(history, request, history.Count, request.ContentId);
        }

        private ContentHistoryItem Convert(IHistorical content)
        {
            var canRestore = content.Status == ContentStatus.Archived;

            var contentType = content.GetType();
            if (contentType == typeof(HtmlContentWidget) || contentType == typeof(ServerControlWidget))
            {
                canRestore = canRestore && userHasAdministrationRole;
            }
            else
            {
                canRestore = canRestore && userHasPublishRole;
            }            

            return new ContentHistoryItem
                       {
                           Id = content.Id,
                           Version = content.Version,
                           StatusName = historyService.GetStatusName(content.Status),
                           Status = content.Status,
                           ArchivedByUser = content.Status == ContentStatus.Archived ? content.CreatedByUser : null,
                           ArchivedOn = content.Status == ContentStatus.Archived ? content.CreatedOn : (DateTime?)null,
                           DisplayedFor = content.Status == ContentStatus.Archived && content.PublishedOn != null
                                   ? content.CreatedOn - content.PublishedOn.Value
                                   : (TimeSpan?)null,
                           PublishedByUser = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedByUser : null,
                           PublishedOn = content.Status == ContentStatus.Published || content.Status == ContentStatus.Archived ? content.PublishedOn : null,
                           CreatedOn = content.CreatedOn,
                           CanCurrentUserRestoreIt = canRestore
                       };
        }
    }
}