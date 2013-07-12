using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Redirect.DeleteRedirect
{
    public class DeleteRedirectCommand : CommandBase, ICommand<SiteSettingRedirectViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(SiteSettingRedirectViewModel request)
        {
            var redirect = Repository.Delete<Models.Redirect>(request.Id, request.Version);
            UnitOfWork.Commit();

            Events.PageEvents.Instance.OnRedirectDeleted(redirect);

            return true;
        }
    }
}