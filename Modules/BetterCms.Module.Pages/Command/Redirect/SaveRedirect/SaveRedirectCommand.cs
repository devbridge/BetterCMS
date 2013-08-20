using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Redirect.SaveRedirect
{
    public class SaveRedirectCommand : CommandBase, ICommand<SiteSettingRedirectViewModel, SiteSettingRedirectViewModel>
    {
        /// <summary>
        /// The redirect service
        /// </summary>
        private readonly IRedirectService redirectService;

        /// <summary>
        /// The url service
        /// </summary>
        private readonly IUrlService urlService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveRedirectCommand" /> class.
        /// </summary>
        /// <param name="redirectService">The redirect service.</param>
        /// <param name="urlService">The URL service.</param>
        public SaveRedirectCommand(IRedirectService redirectService, IUrlService urlService)
        {
            this.redirectService = redirectService;
            this.urlService = urlService;
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
            Models.Redirect redirect;

            request.PageUrl = urlService.FixUrl(request.PageUrl);
            request.RedirectUrl = urlService.FixUrl(request.RedirectUrl);

            // Validate request
            if (!urlService.ValidateUrl(request.PageUrl))
            {
                var message = PagesGlobalization.SaveRedirect_InvalidPageUrl_Message;
                var logMessage = string.Format("Invalid page url {0}.", request.PageUrl);
                throw new ValidationException(() => message, logMessage);
            }
            if (!urlService.ValidateUrl(request.RedirectUrl))
            {
                var message = PagesGlobalization.SaveRedirect_InvalidRedirectUrl_Message;
                var logMessage = string.Format("Invalid redirect url {0}.", request.RedirectUrl);
                throw new ValidationException(() => message, logMessage);
            }

            // Validate for url patterns
            string patternsValidationMessage;
            if (!urlService.ValidateUrlPatterns(request.PageUrl, out patternsValidationMessage))
            {
                var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, request.PageUrl);
                throw new ValidationException(() => patternsValidationMessage, logMessage);
            }
            if (!urlService.ValidateUrlPatterns(request.RedirectUrl, out patternsValidationMessage, PagesGlobalization.SaveRedirect_RedirectUrl_Name))
            {
                var logMessage = string.Format("{0}. URL: {1}.", patternsValidationMessage, request.PageUrl);
                throw new ValidationException(() => patternsValidationMessage, logMessage);
            }

            redirectService.ValidateRedirectExists(request.PageUrl, request.Id);
            redirectService.ValidateForCircularLoop(request.PageUrl, request.RedirectUrl, request.Id);

            if (request.Id.HasDefaultValue())
            {
                redirect = new Models.Redirect();
            }
            else
            {
                redirect = Repository.First<Models.Redirect>(request.Id);
            }

            redirect.Version = request.Version;
            redirect.PageUrl = request.PageUrl;
            redirect.RedirectUrl = request.RedirectUrl;

            Repository.Save(redirect);
            UnitOfWork.Commit();

            // Notify.
            if (request.Id.HasDefaultValue())
            {
                Events.PageEvents.Instance.OnRedirectCreated(redirect);
            }
            else
            {
                Events.PageEvents.Instance.OnRedirectUpdated(redirect);
            }

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