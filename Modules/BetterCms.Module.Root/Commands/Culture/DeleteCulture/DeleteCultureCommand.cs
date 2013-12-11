using BetterCms.Core.Mvc.Commands;
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
            var culture = Repository.Delete<Models.Culture>(request.Id, request.Version);
            UnitOfWork.Commit();

            Events.RootEvents.Instance.OnCultureDeleted(culture);

            return true;
        }
    }
}