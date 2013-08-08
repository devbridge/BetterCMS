using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.Services
{
    public class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultOptionService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Merges the options and values to one lsit of option value view models.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <param name="forEdit">if set to <c>true</c> values are merged for edit.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public IList<OptionValueViewModel> MergeOptionsAndValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues, bool forEdit)
        {
            var optionModels = new List<OptionValueViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    IOption option = null;
                    if (options != null)
                    {
                        option = options.FirstOrDefault(f => f.Key.Trim().Equals(optionValue.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                    }

                    var optionViewModel = new OptionValueViewModel
                                              {
                                                  Type = optionValue.Type,
                                                  OptionKey = optionValue.Key.Trim(),
                                                  OptionValue = optionValue.Value,
                                                  OptionDefaultValue = option != null ? option.Value : null
                                              };

                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    if (!forEdit && string.IsNullOrWhiteSpace(option.Value))
                    {
                        continue;
                    }

                    if (!optionModels.Any(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        optionModels.Add(new OptionValueViewModel
                        {
                            Type = option.Type,
                            OptionKey = option.Key.Trim(),
                            OptionValue = forEdit ? null : option.Value,
                            OptionDefaultValue = option.Value
                        });
                    }
                }
            }

            return optionModels.OrderBy(o => o.OptionKey).ToList();
        }

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptions">The saved options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        public void SaveOptionValues<TEntity>(IEnumerable<OptionValueViewModel> optionViewModels, IEnumerable<TEntity> savedOptions,
            Func<TEntity> entityCreator)
            where TEntity : Entity, IOption
        {
            foreach (var optionViewModel in optionViewModels)
            {
                var savedOption = savedOptions.FirstOrDefault(f => f.Key.Trim().Equals(optionViewModel.OptionKey.Trim(), StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(optionViewModel.OptionValue))
                {
                    if (savedOption == null)
                    {
                        savedOption = entityCreator();
                        savedOption.Key = optionViewModel.OptionKey;
                    }
                    savedOption.Value = optionViewModel.OptionValue;
                    savedOption.Type = optionViewModel.Type;

                    ValidateOptionValue(savedOption);

                    if (optionViewModel.OptionValue != optionViewModel.OptionDefaultValue)
                    {
                        repository.Save(savedOption);
                    }
                }
                else if (savedOption != null)
                {
                    repository.Delete(savedOption);
                }
            }
        }

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        public void ValidateOptionValue(IOption option)
        {
            if (option != null && !string.IsNullOrWhiteSpace(option.Value))
            {
                try
                {
                    ConvertValueToCorrectType(option.Value, option.Type);
                }
                catch
                {
                    var message = string.Format(RootGlobalization.Option_Invalid_Message,
                        option.Key,
                        option.Type.ToGlobalizationString());

                    throw new ValidationException(() => message, message);
                }
            }
        }

        /// <summary>
        /// Converts option value to the correct value type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <returns>Value, converted to correct type</returns>
        private object ConvertValueToCorrectType(string value, OptionType type)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            switch (type)
            {
                case OptionType.Text:
                    return value;

                case OptionType.Integer:
                    return Convert.ToInt64(value);

                case OptionType.Float:
                    return Convert.ToDecimal(value);

                case OptionType.DateTime:
                    return Convert.ToDateTime(value);

                case OptionType.Boolean:
                    return Convert.ToBoolean(value);

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }
    }
}