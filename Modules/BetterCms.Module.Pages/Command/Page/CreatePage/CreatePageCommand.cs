// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreatePageCommand.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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