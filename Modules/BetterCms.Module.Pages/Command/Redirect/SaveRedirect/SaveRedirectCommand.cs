using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.Redirect.SaveRedirect
{
    public class SaveRedirectCommand : CommandBase, ICommand<SiteSettingRedirectViewModel, SiteSettingRedirectViewModel>
    {
        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveRedirectCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        public SaveRedirectCommand(IRedirectService redirectService)
        {
            this.redirectService = redirectService;
        }

        /// <summary>
        /// Executes a command to save a redirect.
        /// </summary>
        /// <param name="request">The redirect view model.</param>
        /// <returns>
        /// Saved redirect view model.
        /// </returns>
        public SiteSettingRedirectViewModel Execute(SiteSettingRedirectViewModel request)
        {
            var redirect = redirectService.SaveRedirect(request);

            return new SiteSettingRedirectViewModel
                {
                    Id = redirect.Id,
                    Version = redirect.Version,
                    PageUrl = redirect.PageUrl,
                    RedirectUrl = redirect.RedirectUrl
                };
        }
    }
}