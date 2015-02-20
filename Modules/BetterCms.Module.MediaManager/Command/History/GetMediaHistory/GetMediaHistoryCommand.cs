using System;
using System.Linq;

using BetterCms.Core.Security;

using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Models.Enum;
using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels.History;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.MediaManager.Command.History.GetMediaHistory
{
    /// <summary>
    /// Command to load a list of the content history versions.
    /// </summary>
    public class GetMediaHistoryCommand : CommandBase, ICommand<GetMediaHistoryRequest, MediaHistoryViewModel>
    {
        /// <summary>
        /// Gets or sets the history service.
        /// </summary>
        /// <value>
        /// The history service.
        /// </value>
        private readonly IMediaHistoryService historyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMediaHistoryCommand"/> class.
        /// </summary>
        /// <param name="historyService">The history service.</param>
        public GetMediaHistoryCommand(IMediaHistoryService historyService)
        {
            this.historyService = historyService;
        }

        /// <summary>
        /// Gets or sets a value indicating whether user has edit role.
        /// </summary>
        /// <value>
        ///   <c>true</c> if user has edit role; otherwise, <c>false</c>.
        /// </value>
        private bool UserHasEditRole { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user has administration role.
        /// </summary>
        /// <value>
        /// <c>true</c> if user has administration role; otherwise, <c>false</c>.
        /// </value>
        private bool UserHasAdministrationRole { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>View model with media history items.</returns>
        public MediaHistoryViewModel Execute(GetMediaHistoryRequest request)
        {
            UserHasEditRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent);
            UserHasAdministrationRole = SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration);

            var history = historyService.GetMediaHistory(request.MediaId, request).Select(Convert).ToList();
            return new MediaHistoryViewModel(history, request, history.Count, request.MediaId);
        }

        /// <summary>
        /// Converts the specified media.
        /// </summary>
        /// <param name="media">The media.</param>
        /// <returns>Converts media entity to view model.</returns>
        private MediaHistoryItem Convert(Media media)
        {
            var file = media.Original as MediaFile;

            var canRestore = media.Original != null 
                && (SecurityService.IsAuthorized(RootModuleConstants.UserRoles.EditContent) || SecurityService.IsAuthorized(RootModuleConstants.UserRoles.Administration))
                && (file == null || AccessControlService.GetAccessLevel(file, Context.Principal) >= AccessLevel.ReadWrite);

            return new MediaHistoryItem
                       {
                           Id = media.Id,
                           Version = media.Version,
                           StatusName = media.Original != null
                                ? MediaGlobalization.MediaHistory_Status_Archived
                                : MediaGlobalization.MediaHistory_Status_Active,
                           Status = media.Original != null
                                ? MediaHistoryStatus.Archived
                                : MediaHistoryStatus.Active,
                           PublishedOn = media.PublishedOn,
                           PublishedByUser = media.ModifiedByUser,
                           ArchivedOn = media.Original != null ? media.ModifiedOn : (DateTime?)null,
                           DisplayedFor = media.Original != null
                                   ? media.ModifiedOn - media.PublishedOn
                                   : (TimeSpan?)null,
                           CanCurrentUserRestoreIt = canRestore
                       };
        }
    }
}