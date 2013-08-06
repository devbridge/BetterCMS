using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public class DefaultOptionService : IOptionService
    {
        public void MergeOptionsAndValues(IOptionValuesContainer viewModel, IOptions optionsContainer, IOptions optionValuesContainer)
        {
            IList<OptionValueViewModel> options = new List<OptionValueViewModel>();

            if (optionValuesContainer != null)
            {
                foreach (var optionValue in optionValuesContainer.Options.Distinct())
                {
                    IOption option = null;
                    if (optionsContainer.Options != null)
                    {
                        option = optionsContainer.Options.FirstOrDefault(f => f.Key.Trim().Equals(optionValue.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                    }

                    options.Add(new OptionValueViewModel
                    {
                        Type = optionValue.Type,
                        OptionKey = optionValue.Key.Trim(),
                        OptionValue = optionValue.Value,
                        OptionDefaultValue = option != null ? option.Value : null
                    });
                }
            }

            if (optionsContainer.Options != null)
            {
                foreach (var option in optionsContainer.Options.Distinct())
                {
                    if (!options.Any(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        options.Add(new OptionValueViewModel
                        {
                            Type = option.Type,
                            OptionKey = option.Key.Trim(),
                            OptionValue = null,
                            OptionDefaultValue = option.Value
                        });
                    }
                }
            }

            viewModel.OptionValues = options.OrderBy(o => o.OptionKey).ToList();
        }
    }
}