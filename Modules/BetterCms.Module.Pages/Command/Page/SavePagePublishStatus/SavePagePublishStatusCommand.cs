using System;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.SavePagePublishStatus
{
    public class SavePagePublishStatusCommand : CommandBase, ICommand<SavePagePublishStatusRequest, bool>
    {
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
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
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
                page.IsPublished = request.IsPublished;
                if (request.IsPublished)
                {
                    page.PublishedOn = DateTime.Now;
                }

                Repository.Save(page);
                UnitOfWork.Commit();

            }

            return true;
        }
    }
}