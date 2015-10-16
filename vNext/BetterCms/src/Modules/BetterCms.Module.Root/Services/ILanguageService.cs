using System.Collections.Generic;
using System.Transactions;

using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Services
{
    public interface ILanguageService
    {
        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns>List of language lookup values.</returns>
        IEnumerable<LookupKeyValue> GetLanguagesLookupValues();

        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Language> GetLanguages();
        
        /// <summary>
        /// Gets the invariant language model.
        /// </summary>
        /// <returns>Invariant language model</returns>
        LookupKeyValue GetInvariantLanguageModel();
    }
}