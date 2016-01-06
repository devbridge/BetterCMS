using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Command.Page.SavePageProperties;
using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

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
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// The master page service
        /// </summary>
        private readonly IMasterPageService masterPageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePageCommand" /> class.
        /// </summary>
        /// <param name="pageService">The page service.</param>
        /// <param name="urlService">The URL service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="masterPageService">The master page service.</param>
        public CreatePageCommand(IPageService pageService, IUrlService urlService, ICmsConfiguration cmsConfiguration, IOptionService optionService, IMasterPageService masterPageService)
        {
            this.pageService = pageService;
            this.urlService = urlService;
            this.cmsConfiguration = cmsConfiguration;
            this.optionService = optionService;
            this.masterPageService = masterPageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>Created page</returns>
        public virtual SavePageResponse Execute(AddNewPageViewModel request)
        {
            if (request.CreateMasterPage)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            if (!request.MasterPageId.HasValue && !request.TemplateId.HasValue)
            {
                var message = RootGlobalization.MasterPage_Or_Layout_ShouldBeSelected_ValidationMessage;
                throw new ValidationException(() => message, message);
            }
            if (request.MasterPageId.HasValue && request.TemplateId.HasValue)
            {
                var logMessage = string.Format("Only one of master page and layout can be selected. LayoutId: {0}, MasterPageId: {1}", request.MasterPageId, request.TemplateId);
                var message = RootGlobalization.MasterPage_Or_Layout_OnlyOne_ShouldBeSelected_ValidationMessage;
                throw new ValidationException(() => message, logMessage);
            }

            // Create / fix page url.
            var pageUrl = request.PageUrl;
            var createPageUrl = pageUrl == null;

            if (createPageUrl && !string.IsNullOrWhiteSpace(request.PageTitle))
            {
                pageUrl = pageService.CreatePagePermalink(request.PageTitle, request.ParentPageUrl, null, request.LanguageId);
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
                    PageUrlHash = pageUrl.UrlHash(),
                    Title = request.PageTitle,
                    MetaTitle = request.PageTitle,
                    Status = request.CreateMasterPage ? PageStatus.Published : PageStatus.Unpublished,
                    IsMasterPage = request.CreateMasterPage
                };

            if (request.MasterPageId.HasValue)
            {
                page.MasterPage = Repository.AsProxy<Root.Models.Page>(request.MasterPageId.Value);

                masterPageService.SetPageMasterPages(page, request.MasterPageId.Value);
            } 
            else
            {
                page.Layout = Repository.AsProxy<Root.Models.Layout>(request.TemplateId.Value);
            }

            if (cmsConfiguration.EnableMultilanguage)
            {
                if (request.LanguageId.HasValue && !request.LanguageId.Value.HasDefaultValue())
                {
                    page.Language = Repository.AsProxy<Language>(request.LanguageId.Value);
                }
            }

            page.Options = optionService.SaveOptionValues(request.OptionValues, null, () => new PageOption { Page = page });

            Repository.Save(page);

            // Update access control if enabled:
            if (cmsConfiguration.Security.AccessControlEnabled)
            {
                AccessControlService.UpdateAccessControl(page, request.UserAccessList != null ? request.UserAccessList.Cast<IAccessRule>().ToList() : null);
            }

            UnitOfWork.Commit();

            // Notifying, that page is created
            Events.PageEvents.Instance.OnPageCreated(page);

            var response = new SavePageResponse(page) { IsSitemapActionEnabled = ConfigurationHelper.IsSitemapActionEnabledAfterAddingNewPage(cmsConfiguration) };

            return response;
        }
    }
}