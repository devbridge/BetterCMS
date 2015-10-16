using System;
using BetterCms.Module.Root.Commands.Authentication.GetAuthenticationInfo;
using BetterCms.Module.Root.Commands.Authentication.SearchRoles;
using BetterCms.Module.Root.Commands.Authentication.SearchUsers;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using BetterModules.Core.Web.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// User authentication handling controller.
    /// </summary>
    [BcmsAuthorize]
    [Area(RootModuleDescriptor.RootAreaName)]
    public class AuthenticationController : CmsControllerBase
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private readonly ILogger logger;

        public AuthenticationController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<AuthenticationController>();
        }

        /// <summary>
        /// Returns view with user information.
        /// </summary>
        /// <returns>Rendered view with user information.</returns>
        [BcmsAuthorize]
        public ActionResult Info()
        {
            var model = GetCommand<GetAuthenticationInfoCommand>().Execute();
            
            return View(model);
        }

        /// <summary>
        /// Executes FormsAuthentication.SignOut action and redirects to default page.
        /// </summary>
        /// <returns>Returns redirect action to default page.</returns>
        public ActionResult Logout()
        {
            try
            {
                return SignOutUserIfAuthenticated();
            }
            catch (Exception ex)
            {
                logger.LogError("Failed to logout user {0}.", ex, User.Identity);
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        public ActionResult IsAuthorized(string roles)
        {
            return Json(new WireJson { Success = SecurityService.IsAuthorized(roles) });
        }

        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SuggestRoles(SuggestionViewModel model)
        {
            var suggestedRoles = GetCommand<SearchRolesCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedRoles });
        }

        [BcmsAuthorize(RootModuleConstants.UserRoles.Administration)]
        public ActionResult SuggestUsers(SuggestionViewModel model)
        {
            var suggestedRoles = GetCommand<SearchUsersCommand>().ExecuteCommand(model);

            return Json(new { suggestions = suggestedRoles });
        }
    }
}
