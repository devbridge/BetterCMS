using System;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;


namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    /// <summary>
    /// Command to updated page status.
    /// </summary>
    public class SavePagePublishStatusCommand : CommandBase, ICommand<SavePagePublishStatusRequest, bool>
    {
        /// <summary>
        /// The content service.
        /// </summary>
        private readonly IContentService contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePublishStatusCommand"/> class.
        /// </summary>
        /// <param name="contentService">The content service.</param>
        public SavePagePublishStatusCommand(IContentService contentService)
        {
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if succeeded, otherwise <c>false</c></returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If page status is not correct.</exception>
        public bool Execute(SavePagePublishStatusRequest request)
        {
            AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.PublishContent);     // This would rise security exception if user has no access.

            var page = UnitOfWork.Session
                .QueryOver<PageProperties>().Where(p => p.Id == request.PageId && !p.IsDeleted)
                .SingleOrDefault<PageProperties>();

            if (page != null)
            {
                if (page.IsMasterPage)
                {
                    return false;
                }

                var initialStatus = page.Status;

                if (page.Status == PageStatus.Draft || page.Status == PageStatus.Preview)
                {
                    var message = string.Format(PagesGlobalization.SavePageStatus_PageIsInInappropriateStatus_Message);
                    var logMessage = string.Format("Draft/Preview page id={0} can not be published.", page.Id);
                    throw new ValidationException(() => message, logMessage);
                }

                if (request.IsPublished)
                {
                    page.Status = PageStatus.Published;
                    page.PublishedOn = DateTime.Now;
                }
                else
                {
                    page.Status = PageStatus.Unpublished;
                }

                // NOTE: When transaction is enabled exception is raised from DefaultEntityTrackingService.DemandReadWriteRule() saying that DB timeouted...
                // UnitOfWork.BeginTransaction();

                Repository.Save(page);

                if (request.IsPublished)
                {
                    contentService.PublishDraftContent(page.Id);
                }

                UnitOfWork.Commit();

                if (page.Status != initialStatus)
                {
                    Events.PageEvents.Instance.OnPagePublishStatusChanged(page);
                }
            }

            return true;
        }
    }
}