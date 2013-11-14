using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.Services
{
    public interface IOptionService
    {
        /// <summary>
        /// Merges options and values and returns one list with option value view models for edit (values are returned as strings).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        List<OptionValueEditViewModel> GetMergedOptionValuesForEdit(IEnumerable<IOption> options, IEnumerable<IOption> optionValues);

        /// <summary>
        /// Merges options and values and returns one list with option value view models for use (values are returned as objects).
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        List<IOptionValue> GetMergedOptionValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues);

        /// <summary>
        /// Gets the merged master pages option values.
        /// </summary>
        /// <param name="masterPageId">The master page id.</param>
        /// <param name="templateId">The template id.</param>
        /// <param name="pageId">The page id.</param>
        /// <returns>List of option values view models, merged from page, master page and layout options and option values</returns>
        List<OptionValueEditViewModel> GetMergedMasterPagesOptionValues(Guid pageId, Guid? masterPageId, Guid? templateId);

        /// <summary>
        /// Saves the options - adds new ones and deleted the old ones..
        /// </summary>
        /// <typeparam name="TOption">The type of the option entity.</typeparam>
        /// <typeparam name="TEntity">The type of the option parent entity.</typeparam>
        /// <param name="optionContainer">The options container entity.</param>
        /// <param name="options">The list of new options.</param>
        void SetOptions<TOption, TEntity>(IOptionContainer<TEntity> optionContainer, IEnumerable<IOption> options)
            where TEntity : IEntity
            where TOption : IDeletableOption<TEntity>, new();

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptionValues">The list of saved option values.</param>
        /// <param name="entityCreator">The entity creator.</param>
        void SaveOptionValues<TEntity>(IEnumerable<OptionValueEditViewModel> optionViewModels, IEnumerable<TEntity> savedOptionValues, 
            Func<TEntity> entityCreator)
            where TEntity : Entity, IOption;

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        void ValidateOptionValue(IOption option);

        /// <summary>
        /// Validates the uniqueness of the option keys.
        /// </summary>
        /// <param name="options">The options.</param>
        void ValidateOptionKeysUniqueness(IEnumerable<OptionViewModelBase> options);

        /// <summary>
        /// Loads the custom option entities by specified ids.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns>List of custom option entities</returns>
        List<CustomOption> GetCustomOptionsById(string[] ids);

        /// <summary>
        /// Sets the custom option value titles.
        /// </summary>
        /// <param name="optionModels">The option models.</param>
        /// <param name="valueModels">The value models.</param>
        void SetCustomOptionValueTitles(IEnumerable<OptionViewModel> optionModels, IEnumerable<OptionValueEditViewModel> valueModels = null);

        /// <summary>
        /// Gets the custom options.
        /// </summary>
        /// <returns>List of custom option view models</returns>
        List<CustomOptionViewModel> GetCustomOptions();

        /// <summary>
        /// Gets the custom options future query.
        /// </summary>
        /// <returns>Future query for the list of custom option view models</returns>
        IEnumerable<CustomOptionViewModel> GetCustomOptionsFuture();

        /// <summary>
        /// Fixes the option value for save.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Fixed option value</returns>
        string ClearFixValueForSave(string title, OptionType type, string value);

        /// <summary>
        /// Fixes the option value for edit.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Fixed option value</returns>
        string ClearFixValueForEdit(OptionType type, string value);
    }
}