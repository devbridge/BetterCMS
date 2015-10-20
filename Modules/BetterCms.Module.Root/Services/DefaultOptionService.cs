using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

using BetterCms.Core;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Providers;
using BetterCms.Module.Root.ViewModels.Option;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;
using BetterModules.Core.Web.Services.Caching;

using NHibernate.Linq;
using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Services
{
    public class DefaultOptionService : IOptionService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The cache service.
        /// </summary>
        private readonly ICacheService cacheService;

        /// <summary>
        /// The CMS configuration.
        /// </summary>
        private readonly ICmsConfiguration cmsConfiguration;

        /// <summary>
        /// The cache key.
        /// </summary>
        private const string CacheKey = "bcms-custom-options-list";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cmsConfiguration">The cms configuration.</param>
        public DefaultOptionService(IRepository repository, ICacheService cacheService, ICmsConfiguration cmsConfiguration)
        {
            this.repository = repository;
            this.cacheService = cacheService;
            this.cmsConfiguration = cmsConfiguration;
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for edit (values are returned as strings).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<OptionValueEditViewModel> GetMergedOptionValuesForEdit(IEnumerable<IOptionEntity> options, IEnumerable<IOptionEntity> optionValues)
        {
            var optionModels = new List<OptionValueEditViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    IOptionEntity option = null;
                    if (options != null)
                    {
                        option = options.FirstOrDefault(
                            f => f.Key.Trim().Equals(optionValue.Key.Trim(), StringComparison.OrdinalIgnoreCase) && f.Type == optionValue.Type);
                    }

                    var optionViewModel = new OptionValueEditViewModel
                                              {
                                                  Type = optionValue.Type,
                                                  CustomOption = optionValue.CustomOption != null ? new CustomOptionViewModel
                                                        {
                                                            Identifier = optionValue.CustomOption.Identifier,
                                                            Title = optionValue.CustomOption.Title,
                                                            Id = optionValue.CustomOption.Id
                                                        } : null,
                                                  OptionKey = optionValue.Key.Trim(),
                                                  OptionValue = ClearFixValueForEdit(optionValue.Type, optionValue.Value),
                                                  OptionDefaultValue = option != null ? ClearFixValueForEdit(option.Type, option.Value) : null,
                                                  UseDefaultValue = option != null && optionValue.Value == null // false
                                              };
                    if (cmsConfiguration.EnableMultilanguage && optionValue is IMultilingualOption)
                    {
                        var translations = new List<OptionTranslationViewModel>();
                        var multiLangOpt = optionValue as IMultilingualOption;
                        if (multiLangOpt.Translations != null)
                        {
                            foreach (var optionTranslation in multiLangOpt.Translations)
                            {
                                var translation = new OptionTranslationViewModel
                                {
                                    LanguageId = optionTranslation.LanguageId,
                                    OptionValue = ClearFixValueForEdit(optionValue.Type, optionTranslation.Value)
                                };
                                translations.Add(translation);
                            }
                        }
                        optionViewModel.ValueTranslations = translations;
                    }

                    if (option == null)
                    {
                        optionViewModel.CanEditOption = true;
                    }

                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    if (!optionModels.Any(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        var optionValueViewModel = new OptionValueEditViewModel
                        {
                            Type = option.Type,
                            CustomOption =
                                option.CustomOption != null
                                    ? new CustomOptionViewModel
                                    {
                                        Identifier = option.CustomOption.Identifier,
                                        Title = option.CustomOption.Title,
                                        Id = option.CustomOption.Id
                                    }
                                    : null,
                            OptionKey = option.Key.Trim(),
                            OptionValue = ClearFixValueForEdit(option.Type, option.Value),
                            OptionDefaultValue = ClearFixValueForEdit(option.Type, option.Value),
                            UseDefaultValue = true
                        };

                        if (cmsConfiguration.EnableMultilanguage && option is IMultilingualOption)
                        {
                            var multiLangOpt = option as IMultilingualOption;
                            var translations = new List<OptionTranslationViewModel>();
                            if (multiLangOpt.Translations != null)
                            {
                                foreach (var optionTranslation in multiLangOpt.Translations)
                                {
                                    var translation = new OptionTranslationViewModel
                                    {
                                        LanguageId = optionTranslation.LanguageId,
                                        OptionValue = optionTranslation.Value
                                    };
                                    translations.Add(translation);
                                }
                            }
                            optionValueViewModel.Translations = translations;
                        }
                        optionModels.Add(optionValueViewModel);
                    }
                    else if (cmsConfiguration.EnableMultilanguage && option is IMultilingualOption)
                    {
                        var optionModel = optionModels.First(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                        var multiLangOpt = option as IMultilingualOption;
                        var translations = new List<OptionTranslationViewModel>();
                        if (multiLangOpt.Translations != null)
                        {
                            foreach (var optionTranslation in multiLangOpt.Translations)
                            {
                                var translation = new OptionTranslationViewModel
                                {
                                    LanguageId = optionTranslation.LanguageId,
                                    OptionValue = optionTranslation.Value
                                };
                                translations.Add(translation);
                            }
                        }
                        optionModel.Translations = translations;
                    }
                }
            }

            SetCustomOptionValueTitles(optionModels, optionModels);

            return optionModels.OrderBy(o => o.OptionKey).ToList();
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for use (values are returned as objects).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<IOptionValue> GetMergedOptionValues(IEnumerable<IOptionEntity> options, IEnumerable<IOptionEntity> optionValues, Guid? languageId = null)
        {
            var optionModels = new List<OptionValueViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    var optionViewModel = CreateOptionValueViewModel(optionValue, languageId);
                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    var optionViewModel = optionModels.FirstOrDefault(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (optionViewModel == null)
                    {
                        optionViewModel = CreateOptionValueViewModel(option, languageId);
                        optionModels.Add(optionViewModel);
                    } else if (optionViewModel.OptionValue == null && languageId != null && languageId == Guid.Empty)
                    {
                        var optViewModel = CreateOptionValueViewModel(option, languageId);
                        optionViewModel.OptionValue = optViewModel.OptionValue;
                    }
                }
            }

            return optionModels.OrderBy(o => o.OptionKey).Cast<IOptionValue>().ToList();
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for use (values are returned as objects).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <param name="languageId"></param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<IOptionValue> GetMergedOptionValues(IEnumerable<IOptionValue> options, IEnumerable<IOptionEntity> optionValues, Guid? languageId = null)
        {
            var optionModels = new List<OptionValueViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    var optionViewModel = CreateOptionValueViewModel(optionValue);
                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    var optionViewModel = optionModels.FirstOrDefault(f => f.OptionKey.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (optionViewModel == null)
                    {
                        optionViewModel = CreateOptionValueViewModel(option);
                        optionModels.Add(optionViewModel);
                    }
                }
            }

            return optionModels.OrderBy(o => o.OptionKey).Cast<IOptionValue>().ToList();
        }

        /// <summary>
        /// Creates the option value view model.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <param name="languageId">The language identifier.</param>
        /// <returns>Created option value view model</returns>
        private OptionValueViewModel CreateOptionValueViewModel(IOptionEntity option, Guid? languageId = null)
        {
            var value = GetValueSafe(option);

            var optionViewModel = new OptionValueViewModel
                {
                    Type = option.Type,
                    OptionKey = option.Key.Trim(),
                    OptionValue = value,
                    CustomOption = option.CustomOption != null ? new CustomOptionViewModel
                    {
                        Identifier = option.CustomOption.Identifier,
                        Title = option.CustomOption.Title,
                        Id = option.CustomOption.Id
                    } : null,
                };
            if (cmsConfiguration.EnableMultilanguage && option is IMultilingualOption && languageId.HasValue)
            {
                var multilangualOption = (IMultilingualOption)option;
                var translation = multilangualOption.Translations.FirstOrDefault(m => m.LanguageId == languageId.ToString());
                if (translation != null)
                {
                    optionViewModel.OptionValue = ConvertValueToCorrectType(translation.Value, option.Type, option.CustomOption);
                }
            }
            return optionViewModel;
        }
        
        /// <summary>
        /// Creates the option value view model.
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>Created option value view model</returns>
        private OptionValueViewModel CreateOptionValueViewModel(IOptionValue option)
        {
            var optionViewModel = new OptionValueViewModel
                {
                    Type = option.Type,
                    OptionKey = option.Key.Trim(),
                    OptionValue = option.Value,
                    CustomOption = option.CustomOption != null ? new CustomOptionViewModel
                    {
                        Identifier = option.CustomOption.Identifier,
                        Title = option.CustomOption.Title,
                        Id = option.CustomOption.Id
                    } : null,
                };

            return optionViewModel;
        }

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="optionValues">The saved options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        /// <param name="translationEntityCreator"></param>
        public IList<TEntity> SaveOptionValues<TEntity>(
            IEnumerable<OptionValueEditViewModel> optionViewModels,
            IEnumerable<TEntity> optionValues,
            Func<TEntity> entityCreator,
            Func<IOptionTranslationEntity> translationEntityCreator = null) where TEntity : Entity, IOptionEntity
        {
            var savedOptionValues = new List<TEntity>();

            if (optionViewModels == null)
            {
                optionViewModels = new List<OptionValueEditViewModel>();
            }

            ValidateOptionKeysUniqueness(optionViewModels);

            var customOptions = LoadAndValidateCustomOptions(optionViewModels);

            if (optionValues != null)
            {
                foreach (var del in optionValues.Where(sov => optionViewModels.All(ovm => ovm.OptionKey != sov.Key)).ToList())
                {
                    if (cmsConfiguration.EnableMultilanguage)
                    {
                        var delMultilang = del as IMultilingualOption;
                        if (delMultilang != null)
                        {
                            foreach (var optionTranslation in delMultilang.Translations)
                            {
                                var optionTranslationEntity = optionTranslation as IOptionTranslationEntity;
                                if (optionTranslationEntity != null)
                                {
                                    repository.Delete(optionTranslationEntity);
                                }
                            }
                        }
                    }

                    repository.Delete(del);
                }
            }

            foreach (var optionViewModel in optionViewModels)
            {
                TEntity optionValue = null;
                if (optionValues != null)
                {
                    optionValue = optionValues.FirstOrDefault(f => f.Key.Trim().Equals(optionViewModel.OptionKey.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!optionViewModel.UseDefaultValue)
                {
                    if (optionValue == null)
                    {
                        optionValue = entityCreator();
                        optionValue.Key = optionViewModel.OptionKey;
                    }
                    optionValue.Value = ClearFixValueForSave(optionViewModel.OptionKey, optionViewModel.Type, optionViewModel.OptionValue);
                    optionValue.Type = optionViewModel.Type;

                    ValidateOptionValue(optionValue);

                    var multilingualOption = optionValue as IMultilingualOption;
                    if (optionValue is IMultilingualOption && cmsConfiguration.EnableMultilanguage && translationEntityCreator != null)
                    {
                        var viewModelTranslations = optionViewModel.ValueTranslations != null ? optionViewModel.ValueTranslations.Cast<IOptionTranslation>().ToList() : null;
                        multilingualOption.Translations = SetMultilangualTranslations(viewModelTranslations, multilingualOption.Translations, optionValue, translationEntityCreator);
                    }
                    if (optionViewModel.Type == OptionType.Custom)
                    {
                        optionValue.CustomOption = repository.AsProxy<CustomOption>(customOptions.First(co => co.Identifier == optionViewModel.CustomOption.Identifier).Id);
                    }
                    else
                    {
                        optionValue.CustomOption = null;
                    }

                    savedOptionValues.Add(optionValue);
                    repository.Save(optionValue);
                }
                else if (optionValue != null)
                {
                    var multilangOptionValue = optionValue as IMultilingualOption;

                    if (cmsConfiguration.EnableMultilanguage && multilangOptionValue != null && translationEntityCreator != null)
                    {
                        var viewModelTranslations = optionViewModel.ValueTranslations != null ? optionViewModel.ValueTranslations.Cast<IOptionTranslation>().ToList() : null;
                        var multilingualTranslations = SetMultilangualTranslations(viewModelTranslations, multilangOptionValue.Translations, optionValue, translationEntityCreator);

                        if (multilingualTranslations.Any())
                        {
                            multilangOptionValue.Translations = multilingualTranslations;
                            optionValue.Value = null;
                            savedOptionValues.Add(optionValue);
                            repository.Save(optionValue);
                        }
                        else
                        {
                            repository.Delete(optionValue);
                        }
                    }
                    else
                    {
                        repository.Delete(optionValue);
                    }
                }
                else
                {
                    optionValue = entityCreator();
                    var multilangOptionValue = optionValue as IMultilingualOption;
                    if (cmsConfiguration.EnableMultilanguage && multilangOptionValue != null && translationEntityCreator != null)
                    {
                        optionValue.Type = optionViewModel.Type;
                        optionValue.CustomOption = optionViewModel.CustomOption;
                        optionValue.Key = optionViewModel.OptionKey;
                        var viewModelTranslations = optionViewModel.ValueTranslations != null ? optionViewModel.ValueTranslations.Cast<IOptionTranslation>().ToList() : null;
                        var multilingualTranslations = SetMultilangualTranslations(viewModelTranslations, multilangOptionValue.Translations, optionValue, translationEntityCreator);

                        if (multilingualTranslations.Any())
                        {
                            multilangOptionValue.Translations = multilingualTranslations;
                            optionValue.Value = null;
                            savedOptionValues.Add(optionValue);
                            repository.Save(optionValue);
                        }
                    }
                }
            }

            return savedOptionValues;
        }

        public string ClearFixValueForSave(string title, OptionType type, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    switch (type)
                    {
                        case OptionType.DateTime:
                            return Convert.ToDateTime(value, Thread.CurrentThread.CurrentCulture.DateTimeFormat)
                                          .ToString("yyyy-MM-dd"); // ISO 8601
                        default:
                            return value;
                    }
                }
                catch
                {
                    var message = string.Format(RootGlobalization.Option_Invalid_Message, title, type.ToGlobalizationString());

                    throw new ValidationException(() => message, message);
                }
            }

            return null;
        }

        public string ClearFixValueForEdit(OptionType type, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    switch (type)
                    {
                        case OptionType.DateTime:
                                return DateTime.ParseExact(value, "yyyy-MM-dd", null) // ISO 8601
                                               .ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
                        default:
                            return value;
                    }
                }
                catch
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Saves the options - adds new ones and deleted the old ones..
        /// </summary>
        /// <typeparam name="TOption">The type of the option entity.</typeparam>
        /// <typeparam name="TEntity">The type of the option parent entity.</typeparam>
        /// <param name="optionContainer">The options container entity.</param>
        /// <param name="options">The list of new options.</param>
        /// <param name="translationEntityCreator"></param>
        public void SetOptions<TOption, TEntity>(IOptionContainer<TEntity> optionContainer, IEnumerable<IOption> options, Func<IOptionTranslationEntity> translationEntityCreator = null)
            where TEntity : IEntity
            where TOption : IDeletableOption<TEntity>, new()
        {
            // Delete old ones
            var removedOptions = new List<IDeletableOption<TEntity>>();
            if (optionContainer.Options != null)
            {
                foreach (var option in optionContainer.Options.Distinct())
                {
                    if (options == null || options.All(o => o.Key != option.Key))
                    {
                        if (!option.IsDeletable)
                        {
                            var message = string.Format(RootGlobalization.SaveOptions_CannotDeleteOption_Message, option.Key);
                            var logMessage = string.Format("Cannot delete option {0}, because it's marked as non-deletable.", option.Id);
                            throw new ValidationException(() => message, logMessage);
                        }
                        if (option is IMultilingualOption)
                        {
                            var multiLangOption = option as IMultilingualOption;
                            foreach (var translation in multiLangOption.Translations.OfType<IEntity>())
                            {
                                repository.Delete(translation);
                            }
                        }
                        removedOptions.Add(option);
                        repository.Delete(option);
                    }
                }
            }

            // Add new / update existing
            if (options != null)
            {
                var customOptions = LoadAndValidateCustomOptions(options);

                var optionsList = new List<IDeletableOption<TEntity>>();
                if (optionContainer.Options != null)
                {
                    foreach (var option in optionContainer.Options)
                    {
                        if (!removedOptions.Contains(option))
                        {
                            optionsList.Add(option);
                        }
                    }
                }

                foreach (var requestOption in options)
                {
                    TOption option = (TOption)optionsList.FirstOrDefault(o => o.Key == requestOption.Key);

                    if (option == null)
                    {
                        option = new TOption();
                        optionsList.Add(option);
                    }

                    option.Key = requestOption.Key;
                    option.Value = ClearFixValueForSave(requestOption.Key, requestOption.Type, requestOption.Value);
                    option.Type = requestOption.Type;
                    option.Entity = (TEntity)optionContainer;

                    if (requestOption.Type == OptionType.Custom)
                    {
                        option.CustomOption = repository.AsProxy<CustomOption>(customOptions.First(co => co.Identifier == requestOption.CustomOption.Identifier).Id);
                    }
                    else
                    {
                        option.CustomOption = null;
                    }

                    ValidateOptionValue(option);

                    var multilingualOption = option as IMultilingualOption;
                    var multilingualRequestOption = requestOption as IMultilingualOption;
                    if (multilingualRequestOption != null && multilingualOption != null && cmsConfiguration.EnableMultilanguage && translationEntityCreator != null)
                    {
                        multilingualOption.Translations = SetMultilangualTranslations(multilingualRequestOption.Translations, multilingualOption.Translations, option, translationEntityCreator);
                    }
                }

                optionContainer.Options = optionsList;
            }
        }

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        public void ValidateOptionValue(IOption option)
        {
            if (option != null)
            {
                ValidateOptionValue(option.Key, option.Value, option.Type, option.CustomOption);
            }
        }

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="customOption">The custom option.</param>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        public void ValidateOptionValue(string key, string value, OptionType type, ICustomOption customOption)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }
            try
            {
                ConvertValueToCorrectType(value, type, customOption);
            }
            catch
            {
                var message = string.Format(RootGlobalization.Option_Invalid_Message, key, type.ToGlobalizationString());

                throw new ValidationException(() => message, message);
            }
        }

        /// <summary>
        /// Validates the uniqueness of the option keys.
        /// </summary>
        /// <param name="options">The options.</param>
        public void ValidateOptionKeysUniqueness(IEnumerable<OptionViewModelBase> options)
        {
            var duplicateKey = options.GroupBy(option => option.OptionKey).Where(group => group.Count() > 1).Select(group => group.Key).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(duplicateKey))
            {
                var message = string.Format(RootGlobalization.Option_DuplicateKey_Message, duplicateKey);
                throw new ValidationException(() => message, message);
            }
        }

        /// <summary>
        /// Gets the safe value (doesn't fail on exception).
        /// </summary>
        /// <param name="option">The option.</param>
        /// <returns>Value, converted to correct type or null, if conversion is impossible</returns>
        private object GetValueSafe(IOptionEntity option)
        {
            var value = option.Value;
            var type = option.Type;

            try
            {
                return ConvertValueToCorrectType(value, type, option.CustomOption);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Converts option value to the correct value type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="customOption">The custom option.</param>
        /// <returns>
        /// Value, converted to correct type
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        private object ConvertValueToCorrectType(string value, OptionType type, ICustomOption customOption)
        {
            if (string.IsNullOrEmpty(value))
            {
                return GetDefaultValueForType(type, customOption);
            }

            switch (type)
            {
                case OptionType.Text:
                case OptionType.MultilineText:
                    return value;

                case OptionType.JavaScriptUrl:
                case OptionType.CssUrl:
                    return HttpUtility.UrlPathEncode(value);

                case OptionType.Integer:
                    int castedInt;
                    if (Int32.TryParse(value, out castedInt))
                    {
                        return castedInt;
                    }
                    return Convert.ToInt64(value, CultureInfo.InvariantCulture);

                case OptionType.Float:
                    value = value.Replace(",", ".");
                    return Convert.ToDecimal(value, CultureInfo.InvariantCulture);

                case OptionType.DateTime:
                    return DateTime.ParseExact(value, "yyyy-MM-dd", null); // ISO 8601

                case OptionType.Boolean:
                    return Convert.ToBoolean(value);

                case OptionType.Custom:
                    ICustomOptionProvider provider = GetCustomOptionsProvider(customOption);

                    if (provider != null)
                    {
                        return provider.ConvertValueToCorrectType(value);
                    }

                    return value;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }

        /// <summary>
        /// Gets the default value for specified option type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="customOption">The custom option.</param>
        /// <returns>
        /// Default types value
        /// </returns>
        private object GetDefaultValueForType(OptionType type, ICustomOption customOption)
        {
            switch (type)
            {
                case OptionType.Text:
                case OptionType.MultilineText:
                case OptionType.DateTime:
                case OptionType.Integer:
                case OptionType.Float:
                case OptionType.JavaScriptUrl:
                case OptionType.CssUrl:
                    return null;

                case OptionType.Boolean:
                    return false;

                case OptionType.Custom:
                    ICustomOptionProvider provider = GetCustomOptionsProvider(customOption);

                    if (provider != null)
                    {
                        return provider.GetDefaultValueForType();
                    }

                    return null;

                default:
                    throw new NotSupportedException(string.Format("Not supported option type: {0}", type));
            }
        }

        /// <summary>
        /// Loads and validate custom options.
        /// </summary>
        /// <returns>The list of custom option entities</returns>
        private IList<CustomOptionViewModel> LoadAndValidateCustomOptions(IEnumerable<IOption> options)
        {
            // Check if options are valid
            var invalidOption = options.FirstOrDefault(o => o.Type == OptionType.Custom && string.IsNullOrWhiteSpace(o.CustomOption.Identifier));
            if (invalidOption != null)
            {
                throw new InvalidOperationException(string.Format("Custom option type provider must be set for custom type! Option Key: {0}", invalidOption.Key));
            }

            // Get already loaded custom options or option types
            List<CustomOptionViewModel> customOptions = options
                .Where(o => o.Type == OptionType.Custom && o.CustomOption is CustomOption && !(o.CustomOption is IProxy))
                .Select(o => new CustomOptionViewModel
                             {
                                 Identifier = o.CustomOption.Identifier, 
                                 Title = o.CustomOption.Title,
                                 Id = o.CustomOption.Id
                             })
                .ToList();

            // Load missing custom options
            var customOptionsIdentifiers = options
                .Where(o => o.Type == OptionType.Custom 
                    && (!(o.CustomOption is CustomOption) || o.CustomOption is IProxy)
                    && customOptions.All(co => co.Identifier != o.CustomOption.Identifier))
                .Select(o => o.CustomOption.Identifier)
                .Distinct().ToArray();

            if (customOptionsIdentifiers.Length == 0)
            {
                return customOptions;
            }

            customOptions.AddRange(GetCustomOptionsById(customOptionsIdentifiers));

            return customOptions;
        }

        /// <summary>
        /// Loads the custom option entities by specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>
        /// List of custom option entities
        /// </returns>
        public List<CustomOptionViewModel> GetCustomOptionsById(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return new List<CustomOptionViewModel>();
            }

            var hasExc = ids.Any(string.IsNullOrWhiteSpace);
            string notExisting = string.Empty;
            List<CustomOptionViewModel> customOptions;
            if (!hasExc)
            {
                var allCustomOptions = GetCustomOptions();
                customOptions = allCustomOptions.Where(co => ids.Contains(co.Identifier)).ToList();

                // Validate if there are any missing custom options
                notExisting = ids.FirstOrDefault(i => customOptions.All(co => co.Identifier != i));
                hasExc = !customOptions.Any() || notExisting != null;
            }
            else
            {
                customOptions = null;
            }

            if (hasExc)
            {
                throw new InvalidOperationException(string.Format("Custom option provider not found for custom type \"{0}\"!", notExisting));
            }

            return customOptions;
        }

        /// <summary>
        /// Gets the custom options provider.
        /// </summary>
        /// <returns>Custom options provider</returns>
        private ICustomOptionProvider GetCustomOptionsProvider(ICustomOption customOption)
        {
            if (customOption == null)
            {
                return null;
            }

            return CustomOptionsProvider.GetProvider(customOption.Identifier);
        }

        /// <summary>
        /// Sets the custom option value titles.
        /// </summary>
        /// <param name="optionModels">The option models.</param>
        /// <param name="valueModels">The value models.</param>
        public void SetCustomOptionValueTitles(IEnumerable<OptionViewModel> optionModels, IEnumerable<OptionValueEditViewModel> valueModels = null)
        {
            var values = optionModels
                    .Where(m => m.Type == OptionType.Custom && m.CustomOption != null)
                    .Select(m => new { m.CustomOption.Identifier, Value = m.OptionDefaultValue });

            if (valueModels != null)
            {
                values = values.Concat(valueModels
                    .Where(m => m.Type == OptionType.Custom && m.CustomOption != null)
                    .Select(m => new { m.CustomOption.Identifier, Value = m.OptionValue }));
            }

            var groupped = values.Distinct().GroupBy(g => g.Identifier);

            foreach (var group in groupped)
            {
                var provider = CustomOptionsProvider.GetProvider(group.Key);

                if (provider != null)
                {
                    var ids = group.Select(g => g.Value).Distinct().ToArray();
                    var titles = provider.GetTitlesForValues(ids, repository);

                    if (titles != null)
                    {
                        foreach (var pair in titles)
                        {
                            optionModels
                                .Where(g => g.Type == OptionType.Custom && (g.OptionDefaultValue == pair.Key || (pair.Key == string.Empty && string.IsNullOrEmpty(g.OptionDefaultValue))))
                                .ToList()
                                .ForEach(g => g.CustomOptionDefaultValueTitle = pair.Value);

                            if (valueModels != null)
                            {
                                valueModels
                                   .Where(g => g.Type == OptionType.Custom && (g.OptionValue == pair.Key || (pair.Key == string.Empty && string.IsNullOrEmpty(g.OptionValue))))
                                   .ToList()
                                   .ForEach(g => g.CustomOptionValueTitle = pair.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        /// <returns>
        /// List of custom option view models
        /// </returns>
        public List<CustomOptionViewModel> GetCustomOptions()
        {
            return cacheService.Get(CacheKey, TimeSpan.FromSeconds(30), LoadCustomOptions);
        }

        /// <summary>
        /// Gets the custom options future query.
        /// </summary>
        /// <returns>
        /// Future query for the list of custom option view models
        /// </returns>
        public IEnumerable<CustomOptionViewModel> GetCustomOptionsFuture()
        {
            return cacheService.Get(CacheKey, TimeSpan.FromSeconds(30), LoadCustomOptionsFuture);
        }

        /// <summary>
        /// Loads the custom options.
        /// </summary>
        /// <returns>
        /// List of custom option view models
        /// </returns>
        private List<CustomOptionViewModel> LoadCustomOptions()
        {
            return LoadCustomOptionsFuture().ToList();
        }

        /// <summary>
        /// Gets the custom options future query.
        /// </summary>
        /// <returns>
        /// Future query for the list of custom option view models
        /// </returns>
        private IEnumerable<CustomOptionViewModel> LoadCustomOptionsFuture()
        {
            return
                repository.AsQueryable<CustomOption>()
                          .OrderBy(o => o.Title)
                          .Select(o => new CustomOptionViewModel { Identifier = o.Identifier, Title = o.Title, Id = o.Id })
                          .ToFuture();
        }

        /// <summary>
        /// Loads the option values and sets up the list of option value view models.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="masterPageId">The master page id.</param>
        /// <param name="templateId">The template id.</param>
        /// <returns>
        /// List of option values for edit
        /// </returns>
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException"></exception>
        public List<OptionValueEditViewModel> GetMergedMasterPagesOptionValues(Guid pageId, Guid? masterPageId, Guid? templateId)
        {
            if (masterPageId.HasValue && masterPageId.Value.HasDefaultValue())
            {
                masterPageId = null;
            }
            if (templateId.HasValue && templateId.Value.HasDefaultValue())
            {
                templateId = null;
            }
            if (!masterPageId.HasValue && !templateId.HasValue)
            {
                var message = RootGlobalization.MasterPage_Or_Layout_ShouldBeSelected_ValidationMessage;
                throw new ValidationException(() => message, message);
            }
            if (masterPageId.HasValue && templateId.HasValue)
            {
                var logMessage = string.Format("Only one of master page and layout can be selected. LayoutId: {0}, MasterPageId: {1}", masterPageId, templateId);
                var message = RootGlobalization.MasterPage_Or_Layout_OnlyOne_ShouldBeSelected_ValidationMessage;
                throw new ValidationException(() => message, logMessage);
            }

            Guid layoutId;
            var allPages = new List<PageMasterPage>();

            allPages.Add(new PageMasterPage
                {
                    Id = pageId, 
                    MasterPageId = masterPageId, 
                    LayoutId = templateId
                });
            

            if (templateId.HasValue)
            {
                layoutId = templateId.Value;
            }
            else
            {
                // Load ids of all the master pages
                var masterPages = repository
                    .AsQueryable<MasterPage>()
                    .Where(mp => mp.Page.Id == pageId)
                    .Select(mp => new PageMasterPage
                    {
                        Id = mp.Master.Id,
                        MasterPageId = mp.Master.MasterPage.Id,
                        LayoutId = mp.Master.Layout.Id
                    })
                    .ToList();
                allPages.AddRange(masterPages);

                layoutId = masterPages.Where(m => m.LayoutId.HasValue).Select(m => m.LayoutId.Value).FirstOrDefault();
            }

            var layoutOptionsFutureQuery = repository
                .AsQueryable<LayoutOption>()
                .Where(lo => lo.Layout.Id == layoutId)
                .ToFuture();

            // Load options of page and master pages
            var pageIds = allPages.Select(p => p.Id).ToArray();
            var pageOptionsFutureQuery = repository
                .AsQueryable<PageOption>()
                .Where(po => pageIds.Contains(po.Page.Id))
                .ToFuture();
            var allPagesOptions = pageOptionsFutureQuery.ToList();

            var layoutOptions = layoutOptionsFutureQuery.ToList();
            var pageOptions = allPagesOptions.Where(po => po.Page.Id == pageId);

            // Get lowest level options, when going up from master pages to layout
            var masterOptions = GetMergedMasterPagesOptionValues(pageId, allPages, allPagesOptions, layoutOptions, new List<IOptionEntity>());
            return GetMergedOptionValuesForEdit(masterOptions, pageOptions);
        }

        /// <summary>
        /// Gets the list of master pages option values.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="allPages">All pages.</param>
        /// <param name="allPagesOptions">All pages options.</param>
        /// <param name="layoutOptions">The layout options.</param>
        /// <param name="allMasterOptions">All master options.</param>
        /// <returns>
        /// List of master page option values
        /// </returns>
        private List<IOptionEntity> GetMergedMasterPagesOptionValues(Guid? id, List<PageMasterPage> allPages, List<PageOption> allPagesOptions,
            List<LayoutOption> layoutOptions, List<IOptionEntity> allMasterOptions)
        {
            var page = allPages.FirstOrDefault(p => p.Id == id);
            if (page != null)
            {
                if (page.MasterPageId.HasValue)
                {
                    allMasterOptions = GetMergedMasterPagesOptionValues(page.MasterPageId.Value, allPages, allPagesOptions, layoutOptions, allMasterOptions);

                    foreach (var option in allPagesOptions.Where(o => o.Page.Id == page.MasterPageId.Value))
                    {
                        var masterOption = allMasterOptions.FirstOrDefault(o => o.Key == option.Key
                            && o.Type == option.Type
                            && ((o.CustomOption == null && option.CustomOption == null)
                                || (o.CustomOption != null
                                    && option.CustomOption != null
                                    && o.CustomOption.Identifier == option.CustomOption.Identifier)));

                        if (masterOption != null)
                        {
                            allMasterOptions.Remove(masterOption);
                            allMasterOptions.Add(option);
                        }
                        else
                        {
                            allMasterOptions.Add(option);
                        }
                    }
                }
                else if (page.LayoutId.HasValue)
                {
                    // Returning layout options as master option values
                    return layoutOptions.Cast<IOptionEntity>().ToList();
                }
            }

            return allMasterOptions;
        }

        /// <summary>
        /// Saves the child content options..
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="viewModels">The list of view models with provided child content id and option values list.</param>
        /// <param name="requestedStatus">The requested status for saving content.</param>
        public void SaveChildContentOptions(Models.Content content, IList<ContentOptionValuesViewModel> viewModels, 
            ContentStatus requestedStatus)
        {
            if (viewModels == null)
            {
                viewModels = new ContentOptionValuesViewModel[0];
            }

            Models.Content contentToUpdate = null;
            if ((requestedStatus == ContentStatus.Draft || requestedStatus == ContentStatus.Preview) 
                && content != null
                && requestedStatus != content.Status
                && content.History != null)
            {
                contentToUpdate = content.History.FirstOrDefault(c => c.Status == requestedStatus);
            }
            if (contentToUpdate == null)
            {
                contentToUpdate = content;
            }

            if (contentToUpdate != null && contentToUpdate.ChildContents != null)
            {
                foreach (var childContent in contentToUpdate.ChildContents)
                {
                    var viewModel = viewModels.FirstOrDefault(vm => vm.OptionValuesContainerId == childContent.AssignmentIdentifier);
                    if (viewModel == null)
                    {
                        continue;
                    }
                    
                    IList<OptionValueEditViewModel> optionValues = viewModel.OptionValues;
                    IList<ChildContentOption> childContentOptions = null;

                    if (childContent.Options != null)
                    {
                        childContentOptions = childContent.Options.Distinct().ToList();
                    }

                    SaveOptionValues(optionValues, childContentOptions, () => new ChildContentOption { ChildContent = childContent }, () => new ChildContentOptionTranslation());
                }
            }
        }

        /// <summary>
        /// Gets the child content options.
        /// </summary>
        /// <param name="contentId">The content identifier.</param>
        /// <returns></returns>
        public IList<ContentOptionValuesViewModel> GetChildContentsOptionValues(Guid contentId)
        {
            var models = new List<ContentOptionValuesViewModel>();
            var allChildContents = repository.AsQueryable<ChildContent>()
                .Where(f => f.Parent.Id == contentId && !f.IsDeleted && !f.Child.IsDeleted)
                .OrderBy(f => f.AssignmentIdentifier)
                .Fetch(f => f.Child)
                .ThenFetchMany(f => f.ContentOptions)
                .ThenFetch(f => f.CustomOption)
                .FetchMany(f => f.Options)
                .ThenFetch(f => f.CustomOption)
                .ToList();

            foreach (var childContent in allChildContents)
            {
                var model = new ContentOptionValuesViewModel { OptionValuesContainerId = childContent.AssignmentIdentifier };
                model.OptionValues = GetMergedOptionValuesForEdit(childContent.Child.ContentOptions, childContent.Options);

                if (model.OptionValues.Any())
                {
                    models.Add(model);
                }
            }

            return models;
        }

        /// <summary>
        /// Helper class for storing page, master page and layout ids
        /// </summary>
        private class PageMasterPage
        {
            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            /// <value>
            /// The id.
            /// </value>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the master page id.
            /// </summary>
            /// <value>
            /// The master page id.
            /// </value>
            public Guid? MasterPageId { get; set; }

            /// <summary>
            /// Gets or sets the layout id.
            /// </summary>
            /// <value>
            /// The layout id.
            /// </value>
            public Guid? LayoutId { get; set; }
        }

        private IList<IOptionTranslation> SetMultilangualTranslations(IList<IOptionTranslation> translations, IList<IOptionTranslation> multilingualTranslations, IOptionEntity optionValue, Func<IOptionTranslationEntity> translationEntityCreator)
        {
            var optionsToRemove = new List<IOptionTranslation>();
            if (translations != null)
            {
                foreach (var mt in multilingualTranslations)
                {
                    if (translations.All(x => x.LanguageId != mt.LanguageId))
                    {
                        optionsToRemove.Add(mt);
                    }
                }

                foreach (var optionTranslation in optionsToRemove)
                {
                    multilingualTranslations.Remove(optionTranslation);
                    var optionTranslationEntity = optionTranslation as IOptionTranslationEntity;
                    if (optionTranslationEntity != null)
                    {
                        repository.Delete(optionTranslationEntity);
                    }
                }

                foreach (var t in translations)
                {
                    var optionTranslation = (IOptionTranslationEntity)multilingualTranslations.FirstOrDefault(x => x is IOptionTranslationEntity && x.LanguageId == t.LanguageId);
                    if (optionTranslation == null)
                    {
                        optionTranslation = translationEntityCreator();
                        var languageId = Guid.Parse(t.LanguageId);
                        optionTranslation.Language = repository.AsProxy<Language>(languageId);
                        optionTranslation.Option = optionValue;
                        multilingualTranslations.Add(optionTranslation);
                    }
                    optionTranslation.Value = ClearFixValueForSave(optionValue.Key, optionValue.Type, ((IOptionTranslation)t).Value);
                    ValidateOptionValue(optionValue.Key, optionTranslation.Value, optionValue.Type, optionValue.CustomOption);
                }
            }
            return multilingualTranslations;
        }
    }
}