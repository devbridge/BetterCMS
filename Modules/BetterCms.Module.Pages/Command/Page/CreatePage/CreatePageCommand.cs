using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

namespace BetterCms.Module.Pages.Command.Page.CreatePage
{
    public class CreatePageCommand : CommandBase, ICommand<AddNewPageViewModel, SavePageResponse>
    {
        /// <summary>
        /// The page service
        /// </summary>
        private readonly IPageService pageService;
        
        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePageCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        public CreatePageCommand(IPageService pageService, IUrlService urlService, ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService)
        {
            this.pageService = pageService;
            this.urlService = urlService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>Created page</returns>
        public virtual SavePageResponse Execute(AddNewPageViewModel request)
        {
            // Create / fix page url
            var pageUrl = request.PageUrl;
            var createPageUrl = (pageUrl == null);
            if (createPageUrl && !string.IsNullOrWhiteSpace(request.PageTitle))
            {
                pageUrl = pageService.CreatePagePermalink(request.PageTitle, request.ParentPageUrl);
            }
            else
            {
                pageUrl = urlService.FixUrl(pageUrl);

                // Validate Url
                pageService.ValidatePageUrl(pageUrl);
            }

            var page = new PageProperties
                {
                    PageUrl = pageUrl,
                    PageUrlLowerTrimmed = pageUrl.LowerTrimmedUrl(),
                    Title = request.PageTitle,
                    MetaTitle = request.PageTitle,
                    Layout = Repository.First<Root.Models.Layout>(request.TemplateId),
                    Status = PageStatus.Unpublished
                };
                
            Repository.Save(page);

            // Update access control if enabled:
            if (cmsConfiguration.AccessControlEnabled)
            {
                accessControlService.UpdateAccessControl(request.UserAccessList, page.Id);
            }

            UnitOfWork.Commit();

            // Notifying, that page is created
            Events.PageEvents.Instance.OnPageCreated(page);

            return new SavePageResponse(page);
        }
    }
}