using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Models;
using BetterCms.Core.Services.Caching;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Providers;
using BetterCms.Module.Root.ViewModels.Option;

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
        /// The cache key.
        /// </summary>
        private const string CacheKey = "bcms-custom-options-list";

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultOptionService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="cacheService">The cache service.</param>
        public DefaultOptionService(IRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        /// <summary>
        /// Merges options and values and returns one list with option value view models for edit (values are returned as strings).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<OptionValueEditViewModel> GetMergedOptionValuesForEdit(IEnumerable<IOption> options, IEnumerable<IOption> optionValues)
        {
            var optionModels = new List<OptionValueEditViewModel>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    IOption option = null;
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
                                                            Title = optionValue.CustomOption.Title
                                                        } : null,
                                                  OptionKey = optionValue.Key.Trim(),
                                                  OptionValue = ClearFixValueForEdit(optionValue.Type, optionValue.Value),
                                                  OptionDefaultValue = option != null ? ClearFixValueForEdit(option.Type, option.Value) : null,
                                                  UseDefaultValue = false
                                              };

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
                        optionModels.Add(
                            new OptionValueEditViewModel
                                {
                                    Type = option.Type,
                                    CustomOption = option.CustomOption != null ? new CustomOptionViewModel
                                            {
                                                Identifier = option.CustomOption.Identifier,
                                                Title = option.CustomOption.Title
                                            } : null,
                                    OptionKey = option.Key.Trim(),
                                    OptionValue = ClearFixValueForEdit(option.Type, option.Value),
                                    OptionDefaultValue = ClearFixValueForEdit(option.Type, option.Value),
                                    UseDefaultValue = true
                                });
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
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        public List<IOptionValue> GetMergedOptionValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues)
        {
            var optionModels = new List<IOptionValue>();

            if (optionValues != null)
            {
                foreach (var optionValue in optionValues.Distinct())
                {
                    var value = GetValueSafe(optionValue);

                    var optionViewModel = new OptionValueViewModel
                                              {
                                                  Type = optionValue.Type, 
                                                  OptionKey = optionValue.Key.Trim(), 
                                                  OptionValue = value,
                                                  CustomOption = optionValue.CustomOption != null ? new CustomOptionViewModel
                                                  {
                                                      Identifier = optionValue.CustomOption.Identifier,
                                                      Title = optionValue.CustomOption.Title
                                                  } : null
                                              };
                    optionModels.Add(optionViewModel);
                }
            }

            if (options != null)
            {
                foreach (var option in options.Distinct())
                {
                    if (!optionModels.Any(f => f.Key.Equals(option.Key.Trim(), StringComparison.OrdinalIgnoreCase)))
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
                                                          Title = option.CustomOption.Title
                                                      } : null,
                                                  };
                        optionModels.Add(optionViewModel);
                    }
                }
            }

            return optionModels.OrderBy(o => o.Key).ToList();
        }

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptionValues">The saved options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        public void SaveOptionValues<TEntity>(
            IEnumerable<OptionValueEditViewModel> optionViewModels,
            IEnumerable<TEntity> savedOptionValues,
            Func<TEntity> entityCreator) where TEntity : Entity, IOption
        {
            if (optionViewModels == null)
            {
                optionViewModels = new List<OptionValueEditViewModel>();
            }

            ValidateOptionKeysUniqueness(optionViewModels);

            var customOptions = LoadAndValidateCustomOptions(optionViewModels);

            if (savedOptionValues != null)
            {
                savedOptionValues.Where(sov => optionViewModels.All(ovm => ovm.OptionKey != sov.Key)).ToList().ForEach(del => repository.Delete(del));
            }

            foreach (var optionViewModel in optionViewModels)
            {
                TEntity savedOptionValue = null;
                if (savedOptionValues != null)
                {
                    savedOptionValue = savedOptionValues.FirstOrDefault(f => f.Key.Trim().Equals(optionViewModel.OptionKey.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!optionViewModel.UseDefaultValue)
                {
                    if (savedOptionValue == null)
                    {
                        savedOptionValue = entityCreator();
                        savedOptionValue.Key = optionViewModel.OptionKey;
                    }
                    savedOptionValue.Value = ClearFixValueForSave(optionViewModel.OptionKey, optionViewModel.Type, optionViewModel.OptionValue);
                    savedOptionValue.Type = optionViewModel.Type;

                    ValidateOptionValue(savedOptionValue);

                    if (optionViewModel.Type == OptionType.Custom)
                    {
                        savedOptionValue.CustomOption = customOptions.First(co => co.Identifier == optionViewModel.CustomOption.Identifier);
                    }
                    else
                    {
                        savedOptionValue.CustomOption = null;
                    }

                    repository.Save(savedOptionValue);
                }
                else if (savedOptionValue != null)
                {
                    repository.Delete(savedOptionValue);
                }
            }
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
        public void SetOptions<TOption, TEntity>(IOptionContainer<TEntity> optionContainer, IEnumerable<IOption> options)
            where TEntity : IEntity
            where TOption : IDeletableOption<TEntity>, new()
        {
            // Delete old ones
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
                    optionsList.AddRange(optionContainer.Options);
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
                        option.CustomOption = customOptions.First(co => co.Identifier == requestOption.CustomOption.Identifier);
                    }
                    else
                    {
                        option.CustomOption = null;
                    }

                    ValidateOptionValue(option);
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
            if (option != null && !string.IsNullOrWhiteSpace(option.Value))
            {
                try
                {
                    ConvertValueToCorrectType(option.Value, option.Type, option.CustomOption);
                }
                catch
                {
                    var message = string.Format(RootGlobalization.Option_Invalid_Message, option.Key, option.Type.ToGlobalizationString());

                    throw new ValidationException(() => message, message);
                }
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
        private object GetValueSafe(IOption option)
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
        private IList<CustomOption> LoadAndValidateCustomOptions(IEnumerable<IOption> options)
        {
            // Check if options are valid
            var invalidOption = options.FirstOrDefault(o => o.Type == OptionType.Custom && string.IsNullOrWhiteSpace(o.CustomOption.Identifier));
            if (invalidOption != null)
            {
                throw new InvalidOperationException(string.Format("Custom option type provider must be set for custom type! Option Key: {0}", invalidOption.Key));
            }

            // Get already loaded custom options or option types
            List<CustomOption> customOptions = options
                .Where(o => o.Type == OptionType.Custom && o.CustomOption is CustomOption && !(o.CustomOption is IProxy))
                .Select(o => (CustomOption)o.CustomOption)
                .ToList();

            // Load missing custom options
            var customOptionsIdentifiers = options
                .Where(o => o.Type == OptionType.Custom 
                    && !(o.CustomOption is CustomOption) 
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
        public List<CustomOption> GetCustomOptionsById(string[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return new List<CustomOption>();
            }

            var customOptions = repository
                .AsQueryable<CustomOption>()
                .Where(co => ids.Contains(co.Identifier))
                .ToList();

            // Validate if there are any missing custom options
            var notExisting = ids.FirstOrDefault(i => customOptions.All(co => co.Identifier != i));
            if (notExisting != null)
            {
                throw new InvalidOperationException(string.Format("Custom option provider not found for custom type {0}!", notExisting));
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
                          .Select(o => new CustomOptionViewModel { Identifier = o.Identifier, Title = o.Title })
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
            var masterOptions = GetMergedMasterPagesOptionValues(pageId, allPages, allPagesOptions, layoutOptions, new List<IOption>());
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
        private List<IOption> GetMergedMasterPagesOptionValues(Guid? id, List<PageMasterPage> allPages, List<PageOption> allPagesOptions,
            List<LayoutOption> layoutOptions, List<IOption> allMasterOptions)
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
                    return layoutOptions.Cast<IOption>().ToList();
                }
            }

            return allMasterOptions;
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
    }
}