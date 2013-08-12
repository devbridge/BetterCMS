using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;
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
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptionValues">The list of saved option values.</param>
        /// <param name="parentOptions">The list of parent options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        void SaveOptionValues<TEntity>(IEnumerable<OptionValueEditViewModel> optionViewModels, IEnumerable<TEntity> savedOptionValues, 
            IEnumerable<IOption> parentOptions, Func<TEntity> entityCreator)
            where TEntity : Entity, IOption;

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        void ValidateOptionValue(IOption option);
    }
}