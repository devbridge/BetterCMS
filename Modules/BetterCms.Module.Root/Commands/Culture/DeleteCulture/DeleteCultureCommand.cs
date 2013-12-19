using System.Linq;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Cultures;

namespace BetterCms.Module.Root.Commands.Culture.DeleteCulture
{
    public class DeleteCultureCommand : CommandBase, ICommand<CultureViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if culture is deleted successfully.</returns>
        public bool Execute(CultureViewModel request)
        {
            var culture = Repository.First<Models.Culture>(request.Id);
            if (Repository.AsQueryable<Models.Page>(p => p.Culture == culture).Any())
            {
                var logMessage = string.Format("Cannot delete culture {0}, because it's used in pages.", culture.Name);
                var message = string.Format(RootGlobalization.DeleteCultureCommand_PagesAreUsingCulture_Message, culture.Name);
                throw new ValidationException(() => message, logMessage);
            }

            if (culture.Version != request.Version)
            {
                throw new ConcurrentDataException(culture);
            }

            Repository.Delete(culture);
            UnitOfWork.Commit();

            Events.RootEvents.Instance.OnCultureDeleted(culture);

            return true;
        }
    }
}