using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Pages.Command.Page.AddNewPage
{
    public class AddNewPageCommand : CommandBase, ICommand<AddNewPageCommandRequest, AddNewPageViewModel>
    {
        private readonly ILayoutService layoutService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISecurityService securityService;
        
        private readonly IOptionService optionService;
        
        private readonly IMasterPageService masterPageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewPageCommand" /> class.
        /// </summary>
        /// <param name="LayoutService">The layout service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="masterPageService">The master page service.</param>
        public AddNewPageCommand(ILayoutService LayoutService, ICmsConfiguration cmsConfiguration,
            ISecurityService securityService, IOptionService optionService,
            IMasterPageService masterPageService)
        {
            layoutService = LayoutService;
            this.cmsConfiguration = cmsConfiguration;
            this.securityService = securityService;
            this.optionService = optionService;
            this.masterPageService = masterPageService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AddNewPage view model</returns>
        public AddNewPageViewModel Execute(AddNewPageCommandRequest request)
        {
            if (request.CreateMasterPage)
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.Administration);
            }
            else
            {
                AccessControlService.DemandAccess(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            }

            var principal = securityService.GetCurrentPrincipal();
            var model = new AddNewPageViewModel
                {
                    ParentPageUrl = request.ParentPageUrl,
                    Templates = layoutService.GetAvailableLayouts().ToList(),
                    AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled,
                    UserAccessList = AccessControlService.GetDefaultAccessList(principal).Select(f => new UserAccessViewModel(f)).ToList(),
                    CreateMasterPage = request.CreateMasterPage
                };

            if (model.Templates.Count > 0)
            {
                model.Templates.ToList().ForEach(x => x.IsActive = false);
                var urlHash = request.ParentPageUrl.UrlHash();
                model.Templates.Where(t => t.IsMasterPage && t.MasterUrlHash == urlHash).ToList().ForEach(x => x.IsActive = true);
                if (model.Templates.Count(t => t.IsActive) != 1)
                {
                    model.Templates.First().IsActive = true;
                }

                var active = model.Templates.First(t => t.IsActive);
                if (active != null)
                {
                    if (active.IsMasterPage)
                    {
                        model.MasterPageId = active.TemplateId;
                    }
                    else
                    {
                        model.TemplateId = active.TemplateId;
                    }
                }

                if (model.TemplateId.HasValue)
                {
                    model.OptionValues = layoutService.GetLayoutOptionValues(model.TemplateId.Value);
                }

                if (model.MasterPageId.HasValue)
                {
                    model.OptionValues = masterPageService.GetMasterPageOptionValues(model.MasterPageId.Value);
                }

                model.CustomOptions = optionService.GetCustomOptions();
            }

            return model;
        }
    }
}