// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageHtmlContentCommand.cs" company="Devbridge Group LLC">
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
using System;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Security;

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Content.GetPageHtmlContent
{
    public class GetPageHtmlContentCommand : CommandBase, ICommand<Guid, PageContentViewModel>
    {
        private readonly IContentService contentService;

        private readonly ICmsConfiguration configuration;

        private readonly IMasterPageService masterPageService;

        public GetPageHtmlContentCommand(IContentService contentService, IMasterPageService masterPageService, ICmsConfiguration configuration)
        {
            this.masterPageService = masterPageService;
            this.configuration = configuration;
            this.contentService = contentService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public PageContentViewModel Execute(Guid pageContentId)
        {
            var pageContentForEdit = contentService.GetPageContentForEdit(pageContentId);

            if (pageContentForEdit == null)
            {
                throw new EntityNotFoundException(typeof(PageContent), pageContentId);
            }

            PageContent pageContent = pageContentForEdit.Item1;
            var content = (HtmlContent)pageContentForEdit.Item2;

            var model = new PageContentViewModel 
                                            {
                                                Id = pageContent.Id,
                                                PageId = pageContent.Page.Id,
                                                RegionId = pageContent.Region.Id,
                                                ParentPageContentId = pageContent.Parent != null ? pageContent.Parent.Id : Guid.Empty,
                                                ContentId = pageContent.Content.Id,
                                                ContentName = content.Name,
                                                LiveFrom = content.ActivationDate,
                                                LiveTo = content.ExpirationDate,
                                                PageContent = content.ContentTextMode == ContentTextMode.Html ? content.Html : content.OriginalText,
                                                Version = pageContent.Version,
                                                ContentVersion = pageContent.Content.Version,
                                                CustomCss = content.CustomCss,
                                                CustomJs = content.CustomJs,
                                                EnabledCustomJs = content.UseCustomJs,
                                                EnabledCustomCss = content.UseCustomCss,
                                                EditInSourceMode = content.EditInSourceMode,
                                                EnableInsertDynamicRegion = pageContent.Page.IsMasterPage,
                                                CurrentStatus = content.Status,
                                                HasPublishedContent = content.Original != null,
                                                ContentTextMode = content.ContentTextMode
                                            };

            if (configuration.Security.AccessControlEnabled)
            {
                var accessRules = Repository.AsQueryable<PageContent>()
                                            .Where(x => x.Id == pageContentId && !x.IsDeleted)
                                            .Select(f => f.Page)
                                            .SelectMany(f => f.AccessRules)                                            
                                            .ToList()
                                            .Select(x => new UserAccessViewModel(x)).Cast<IAccessRule>().ToList();

                SetIsReadOnly(model, accessRules);
            }

            model.CanEditContent = SecurityService.IsAuthorized(Context.Principal, RootModuleConstants.UserRoles.EditContent);
            model.CanDestroyDraft = model.CurrentStatus == ContentStatus.Draft && model.HasPublishedContent && model.CanEditContent;

            if (model.EnableInsertDynamicRegion)
            {
                model.LastDynamicRegionNumber = masterPageService.GetLastDynamicRegionNumber();
            }

            return model;
        }
    }
}