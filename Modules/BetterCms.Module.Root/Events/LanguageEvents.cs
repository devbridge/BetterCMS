using System.Collections.Generic;
using System.Linq;

using BetterCms.Module.Root.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents
    {
        /// <summary>
        /// Occurs when a language is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Language>> LanguageCreated;

        /// <summary>
        /// Occurs when a language is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Language>> LanguageUpdated;

        /// <summary>
        /// Occurs when a language is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Language>> LanguageDeleted;

        public void OnLanguageCreated(params Language[] languages)
        {
            if (LanguageCreated != null && languages != null)
            {
                foreach (var language in languages)
                {
                    LanguageCreated(new SingleItemEventArgs<Language>(language));
                }
            }
        }

        public void OnLanguageCreated(IEnumerable<Language> languages)
        {
            if (languages != null)
            {
                OnLanguageCreated(languages.ToArray());
            }
        }

        public void OnLanguageUpdated(Language language)
        {
            if (LanguageUpdated != null)
            {
                LanguageUpdated(new SingleItemEventArgs<Language>(language));
            }
        }

        public void OnLanguageDeleted(Language language)
        {
            if (LanguageDeleted != null)
            {
                LanguageDeleted(new SingleItemEventArgs<Language>(language));
            }        
        }
    }
}
