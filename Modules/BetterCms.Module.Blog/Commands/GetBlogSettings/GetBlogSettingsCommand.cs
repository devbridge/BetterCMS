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