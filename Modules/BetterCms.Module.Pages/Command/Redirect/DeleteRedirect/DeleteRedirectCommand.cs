using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Redirect.DeleteRedirect
{
    public class DeleteRedirectCommand : CommandBase, ICommand<SiteSettingRedirectViewModel, bool>
    {
        private readonly IRedirectService redirectService;

        public DeleteRedirectCommand(IRedirectService redirectService)
        {
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(SiteSettingRedirectViewModel request)
        {
            redirectService.DeleteRedirect(request.Id, request.Version);

            return true;
        }
    }
}