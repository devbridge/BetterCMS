using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Models.Extensions;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.SuggestLanguages
{
    /// <summary>
    /// A command to get languages list by filter.
    /// </summary>
    public class SuggestLanguagesCommand : CommandBase, ICommand<SuggestionViewModel, List<LookupKeyValue>>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// A list of languages.
        /// </returns>
        public List<LookupKeyValue> Execute(SuggestionViewModel model)
        {
            var query = model.Query.ToLowerInvariant();

            var alreadyAdded = Repository.AsQueryable<Models.Language>().Select(c => c.Code).ToList();
            alreadyAdded.Add(System.Globalization.CultureInfo.InvariantCulture.Name);

            return System.Globalization.CultureInfo
                .GetCultures(System.Globalization.CultureTypes.AllCultures)
                .Where(culture => culture.GetFullName().ToLowerInvariant().Contains(query))
                .Where(cullture => !alreadyAdded.Contains(cullture.Name))
                .OrderBy(culture => culture.Name)
                .Select(culture => new LookupKeyValue { Key = culture.Name, Value = culture.GetFullName() })
                .ToList();
        }
    }
}