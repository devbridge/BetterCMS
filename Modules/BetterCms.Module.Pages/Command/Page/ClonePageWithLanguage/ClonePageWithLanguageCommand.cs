// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClonePageWithLanguageCommand.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataAccess.DataContext;

using BetterCms.Module.Pages.Helpers;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Page.ClonePageWithLanguage
{
    /// <summary>
    /// A command to clone given page.
    /// </summary>
    public class ClonePageWithLanguageCommand : CommandBase, ICommand<ClonePageWithLanguageViewModel, ClonePageWithLanguageViewModel>
    {
        private readonly IPageCloneService cloneService;
        private readonly ICmsConfiguration cmsConfiguration;

        public ClonePageWithLanguageCommand(IPageCloneService cloneService, ICmsConfiguration cmsConfiguration)
        {
            this.cloneService = cloneService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The page view model.</param>
        /// <returns>true if page cloned successfully; false otherwise.</returns>
        public virtual ClonePageWithLanguageViewModel Execute(ClonePageWithLanguageViewModel request)
        {
            var languageGroupIdentifier = Repository
                .AsQueryable<Root.Models.Page>(p => p.Id == request.PageId)
                .Select(p => new { LanguageGroupIdentifier = p.LanguageGroupIdentifier })
                .FirstOne()
                .LanguageGroupIdentifier;
            
            if (!languageGroupIdentifier.HasValue)
            {
                languageGroupIdentifier = System.Guid.NewGuid();
            }

            var languageId = !request.LanguageId.HasValue ? System.Guid.Empty : request.LanguageId.Value;

            var newPage = cloneService.ClonePageWithLanguage(request.PageId, request.PageTitle, 
                request.PageUrl, request.UserAccessList, languageId, languageGroupIdentifier.Value);

            return new ClonePageWithLanguageViewModel
                {
                    PageId = newPage.Id,
                    PageTitle = newPage.Title,
                    PageUrl = newPage.PageUrl,
                    IsMasterPage = newPage.IsMasterPage,
                    LanguageId = request.LanguageId,
                    IsSitemapActionEnabled = ConfigurationHelper.IsSitemapActionEnabledAfterAddingTranslationForPage(cmsConfiguration)
                };
        }
    }
}