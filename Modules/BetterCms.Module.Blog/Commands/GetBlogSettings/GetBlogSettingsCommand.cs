// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetBlogSettingsCommand.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Module.Blog.Content.Resources;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Setting;

using BetterCms.Module.Pages.Models.Enums;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;
using BetterCms.Module.Root.ViewModels.SiteSettings;

using BetterModules.Core.Web.Mvc.Commands;

using MvcContrib.Pagination;

namespace BetterCms.Module.Blog.Commands.GetBlogSettings
{
    public class GetBlogSettingsCommand : CommandBase, ICommand<bool, SearchableGridViewModel<SettingItemViewModel>>
    {
        /// <summary>
        /// The options service
        /// </summary>
        private readonly IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBlogSettingsCommand" /> class.
        /// </summary>
        /// <param name="optionService">The option service.</param>
        public GetBlogSettingsCommand(IOptionService optionService)
        {
            this.optionService = optionService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The list of blog template view models</returns>
        public SearchableGridViewModel<SettingItemViewModel> Execute(bool request)
        {
            // Get current default template
            var option = optionService.GetDefaultOption();

            var response = new SearchableGridViewModel<SettingItemViewModel>() { GridOptions = new SearchableGridOptions() };

            var textMode = ContentTextMode.Html;
            if (option != null)
            {
                textMode = option.DefaultContentTextMode;
            }

            var defaultEditMode = new SettingItemViewModel
            {
                Value = (int)textMode,
                Name = BlogGlobalization.SiteSettings_BlogSettingsTab_DefaultEditMode_Title,
                Key = BlogModuleConstants.BlogPostDefaultContentTextModeKey
            };

            var settings = new List<SettingItemViewModel> { defaultEditMode };
            response.Items = new CustomPagination<SettingItemViewModel>(settings, settings.Count, 100, settings.Count);

            return response;
        }
    }
}