using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

namespace BetterCms.Module.Root.Commands.Culture.SuggestCultures
{
    /// <summary>
    /// A command to get cultures list by filter.
    /// </summary>
    public class SuggestCulturesCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of cultures.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = model.Query.ToLowerInvariant();

            return System.Globalization.CultureInfo
                .GetCultures(System.Globalization.CultureTypes.AllCultures)
                .Except(System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures))
                .Where(culture => culture.Name.ToLower().Contains(query) || culture.EnglishName.ToLower().Contains(query) || culture.NativeName.ToLower().Contains(query))
                .OrderBy(culture => culture.Name)
                .Select(culture => new LookupKeyValue { Key = culture.Name, Value = string.Format("{0} ({1}, {2})", culture.Name, culture.EnglishName, culture.NativeName) })
                .ToList();
        }
    }
}