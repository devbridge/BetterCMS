using System.Linq;

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Language;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.DeleteLanguage
{
    public class DeleteLanguageCommand : CommandBase, ICommand<LanguageViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if language is deleted successfully.</returns>
        public bool Execute(LanguageViewModel request)
        {
            var language = Repository.First<Models.Language>(request.Id);
            if (Repository.AsQueryable<Models.Page>(p => p.Language == language).Any())
            {
                var logMessage = string.Format("Cannot delete language {0}, because it's used in pages.", language.Name);
                var message = string.Format(RootGlobalization.DeleteLanguageCommand_PagesAreUsingLanguage_Message, language.Name);
                throw new ValidationException(() => message, logMessage);
            }

            if (language.Version != request.Version)
            {
                throw new ConcurrentDataException(language);
            }

            Repository.Delete(language);
            UnitOfWork.Commit();

            Events.RootEvents.Instance.OnLanguageDeleted(language);

            return true;
        }
    }
}