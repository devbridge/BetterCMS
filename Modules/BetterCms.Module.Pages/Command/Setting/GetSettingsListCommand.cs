using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Services;
using BetterCms.Module.Pages.ViewModels.Setting;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Grids.GridOptions;

namespace BetterCms.Module.Pages.Command.Setting
{
    public class GetSettingsListCommand : CommandBase, ICommand<SearchableGridOptions, ConfigurationSettingsListViewModel>
    {
        private readonly ISettingsService settingsService;

        public GetSettingsListCommand(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public ConfigurationSettingsListViewModel Execute(SearchableGridOptions request)
        {
            //var activeModules = settingsService.GetActiveModules();
            //var settings = settingsService.GetActiveModulesSettings();
            //var resultList = new List<ConfigurationSettingItemViewModel>();

            //foreach (var setting in settings)
            //{
            //    resultList.Add(new ConfigurationSettingItemViewModel
            //    {
            //        Id = setting.Id,
            //        Version = setting.Version,
            //        Name = setting.Name,
            //        Value = setting.Value,
            //        ModuleId = setting.ModuleId,
            //        ModuleName = activeModules.First(m => m.Id == setting.ModuleId).Name
            //    });
            //}

            //return new ConfigurationSettingsListViewModel(resultList, request, resultList.Count);

            return null;
        }
    }
}