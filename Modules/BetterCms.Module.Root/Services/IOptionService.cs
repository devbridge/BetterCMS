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
        /// Merges the options and values to one lsit of option value view models.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="optionValues">The option values.</param>
        /// <param name="forEdit">if set to <c>true</c> values are merged for edit.</param>
        /// <returns>
        /// List of option values view models, merged from options and option values
        /// </returns>
        IList<OptionValueViewModel> MergeOptionsAndValues(IEnumerable<IOption> options, IEnumerable<IOption> optionValues, bool forEdit);

        /// <summary>
        /// Saves the option values: adds new option values and empty values.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="optionViewModels">The option view models.</param>
        /// <param name="savedOptions">The saved options.</param>
        /// <param name="entityCreator">The entity creator.</param>
        void SaveOptionValues<TEntity>(IEnumerable<OptionValueViewModel> optionViewModels, IEnumerable<TEntity> savedOptions, Func<TEntity> entityCreator)
            where TEntity : Entity, IOption;

        /// <summary>
        /// Validates the option value.
        /// </summary>
        /// <param name="option">The option.</param>
        void ValidateOptionValue(IOption option);
    }
}