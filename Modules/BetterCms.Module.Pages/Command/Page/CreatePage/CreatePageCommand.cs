using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

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

        /// <summary>
        /// The CMS configuration
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The access control service
        /// </summary>
        private readonly IAccessControlService accessControlService;

        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePageCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="optionService">The option service.</param>
        public CreatePageCommand(IPageService pageService, IUrlService urlService, ICmsConfiguration cmsConfiguration,
            IAccessControlService accessControlService, IOptionService optionService)
        {
            this.pageService = pageService;
            this.urlService = urlService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.optionService = optionService;
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

            var parentOptions = Repository
                .AsQueryable<LayoutOption>(o => o.Layout.Id == request.TemplateId)
                .ToList();
            optionService.SaveOptionValues(request.OptionValues, null, parentOptions, () => new PageOption { Page = page });

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