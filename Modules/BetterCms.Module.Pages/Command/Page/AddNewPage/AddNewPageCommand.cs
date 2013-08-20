using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;
using BetterCms.Core.Services;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Pages.Command.Page.AddNewPage
{
    public class AddNewPageCommand : CommandBase, ICommand<AddNewPageCommandRequest, AddNewPageViewModel>
    {
        private readonly ILayoutService layoutService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly IAccessControlService accessControlService;

        private readonly ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewPageCommand" /> class.
        /// </summary>
        /// <param name="LayoutService">The layout service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="accessControlService">The access control service.</param>
        /// <param name="securityService">The security service.</param>
        public AddNewPageCommand(ILayoutService LayoutService, ICmsConfiguration cmsConfiguration, IAccessControlService accessControlService, ISecurityService securityService)
        {
            layoutService = LayoutService;
            this.cmsConfiguration = cmsConfiguration;
            this.accessControlService = accessControlService;
            this.securityService = securityService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>AddNewPage view model</returns>
        public AddNewPageViewModel Execute(AddNewPageCommandRequest request)
        {
            var principal = securityService.GetCurrentPrincipal();
            var model = new AddNewPageViewModel
                            {
                                ParentPageUrl = request.ParentPageUrl,
                                Templates = layoutService.GetLayouts(),
                                AccessControlEnabled = cmsConfiguration.AccessControlEnabled,
                                UserAccessList = accessControlService.GetDefaultAccessList(principal).Cast<UserAccessViewModel>().ToList()
                            };

            if (model.Templates.Count > 0)
            {
                model.Templates.ToList().ForEach(x => x.IsActive = false);
                model.Templates.First().IsActive = true;
                model.TemplateId = model.Templates.First(t => t.IsActive).TemplateId;

                model.OptionValues = layoutService.GetLayoutOptionValues(model.TemplateId);
            }

            return model;
        }
    }
}