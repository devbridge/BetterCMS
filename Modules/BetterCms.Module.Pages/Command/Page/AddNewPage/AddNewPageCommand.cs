// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddNewPageCommand.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess;

using BetterCms.Core.Services;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.AddNewPage
{
    public class AddNewPageCommand : CommandBase, ICommand<AddNewPageCommandRequest, AddNewPageViewModel>
    {
        private readonly ILayoutService layoutService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly ISecurityService securityService;
        
        private readonly IOptionService optionService;
        
        private readonly IMasterPageService masterPageService;

        private readonly IRepository repository;
        
        private readonly ILanguageService languageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewPageCommand" /> class.
        /// </summary>
        /// <param name="LayoutService">The layout service.</param>
        /// <param name="cmsConfiguration">The CMS configuration.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="optionService">The option service.</param>
        /// <param name="masterPageService">The master page service.</param>
        /// <param name="repository">The repository.</param>
        /// <param name="languageService">The language service.</param>
        public AddNewPageCommand(ILayoutService LayoutService, ICmsConfiguration cmsConfiguration,
            ISecurityService securityService, IOptionService optionService,
            IMasterPageService masterPageService, IRepository repository, ILanguageService languageService)
        {
            layoutService = LayoutService;
            this.cmsConfiguration = cmsConfiguration;
            this.securityService = securityService;
            this.optionService = optionService;
            this.masterPageService = masterPageService;
            this.repository = repository;
            this.languageService = languageService;
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

            var showLanguages = cmsConfiguration.EnableMultilanguage && !request.CreateMasterPage;
            var languagesFuture = (showLanguages) ? languageService.GetLanguagesLookupValues() : null;

            var principal = securityService.GetCurrentPrincipal();
            var model = new AddNewPageViewModel
                {
                    ParentPageUrl = request.ParentPageUrl,
                    Templates = layoutService.GetAvailableLayouts().ToList(),
                    AccessControlEnabled = cmsConfiguration.Security.AccessControlEnabled,
                    CreateMasterPage = request.CreateMasterPage,
                    ShowLanguages = showLanguages
                };

            if (showLanguages)
            {
                model.Languages = languagesFuture.ToList();
                model.ShowLanguages = model.Languages.Any();
            }

            if (model.Templates.Count > 0)
            {
                model.Templates.ToList().ForEach(x => x.IsActive = false);

                // Select current page as master
                var urlHash = request.ParentPageUrl.UrlHash();
                model.Templates.Where(t => t.IsMasterPage && t.MasterUrlHash == urlHash).ToList().ForEach(x => x.IsActive = true);
                
                // Select current page's layout
                if (model.Templates.Count(t => t.IsActive) != 1)
                {
                    // Try to get layout of the current page
                    var currentPageLayout = repository
                        .AsQueryable<Root.Models.Page>(p => p.PageUrlHash == request.ParentPageUrl.UrlHash())
                        .Select(p => new
                                         {
                                             MasterPageId = p.MasterPage != null ? p.MasterPage.Id : (System.Guid?)null,
                                             LayoutId = p.Layout != null ? p.Layout.Id : (System.Guid?)null
                                         })
                        .FirstOrDefault();
                    if (currentPageLayout != null)
                    {
                        if (currentPageLayout.MasterPageId.HasValue)
                        {
                            model.Templates
                                .Where(t => t.IsMasterPage && t.TemplateId == currentPageLayout.MasterPageId.Value)
                                .Take(1)
                                .ToList().ForEach(x => x.IsActive = true);
                        }
                        else if (currentPageLayout.LayoutId.HasValue)
                        {
                            model.Templates
                                .Where(t => !t.IsMasterPage && t.TemplateId == currentPageLayout.LayoutId.Value)
                                .Take(1)
                                .ToList().ForEach(x => x.IsActive = true);
                        }
                    }
                }

                // Select first layout as active
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
                        model.UserAccessList = Repository
                            .AsQueryable<Root.Models.Page>()
                            .Where(x => x.Id == model.MasterPageId && !x.IsDeleted)
                            .SelectMany(x => x.AccessRules)
                            .OrderBy(x => x.Identity)
                            .Select(x => new UserAccessViewModel(x)).ToList();
                    }
                    else
                    {
                        model.TemplateId = active.TemplateId;
                        model.UserAccessList = AccessControlService.GetDefaultAccessList(principal).Select(f => new UserAccessViewModel(f)).ToList();
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