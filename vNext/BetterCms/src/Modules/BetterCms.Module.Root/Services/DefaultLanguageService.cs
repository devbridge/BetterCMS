using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultLanguageService : ILanguageService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLanguageService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultLanguageService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns>
        /// List of language lookup values.
        /// </returns>
        public IEnumerable<LookupKeyValue> GetLanguagesLookupValues()
        {
            return repository
                .AsQueryable<Language>()
                .OrderBy(c => c.Code)
                .Select(c => new LookupKeyValue
                                 {
                                     Key = c.Id.ToString().ToLowerInvariant(),
                                     Value = c.Name
                                 })
                .ToFuture();
        }

        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns>
        /// List of languages.
        /// </returns>
        public IEnumerable<Language> GetLanguages()
        {
            return repository
                .AsQueryable<Language>()
                .OrderBy(c => c.Code)
                .ToFuture();
        }

        /// <summary>
        /// Gets the invariant language model.
        /// </summary>
        /// <returns>
        /// Invariant language model
        /// </returns>
        public LookupKeyValue GetInvariantLanguageModel()
        {
            return new LookupKeyValue(System.Guid.Empty.ToString(), RootGlobalization.InvariantLanguage_Title);
        }
    }
}