using System;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    /// <summary>
    /// Command to updated page status.
    /// </summary>
    public class SavePagePublishStatusCommand : CommandBase, ICommand<SavePagePublishStatusRequest, bool>
    {
        /// <summary>
        /// The security service.
        /// </summary>
        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SavePagePublishStatusCommand" /> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        public SavePagePublishStatusCommand(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns> <cref>true</cref> if status updated successfully, otherwise <cref>false</cref>.</returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">If user has no rights or page status is inappropriate.</exception>
        public bool Execute(SavePagePublishStatusRequest request)
        {
            var principal = securityService.GetCurrentPrincipal();
            if (!securityService.CanPublishPage(principal))
            {
                var message = string.Format(PagesGlobalization.SavePagePublishStatus_NoPermission_Message);
                var logMessage = string.Format("User has no permission to change page publish status. User: {0}", principal.Identity.Name);
                throw new ValidationException(() => message, logMessage);
            }

            var page = UnitOfWork.Session
                .QueryOver<PageProperties>().Where(p => p.Id == request.PageId && !p.IsDeleted)
                .SingleOrDefault<PageProperties>();

            if (page != null)
            {
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

                Repository.Save(page);
                UnitOfWork.Commit();
            }

            return true;
        }
    }
}