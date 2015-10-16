using System;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Core.Security;

using BetterCms.Module.Root.Commands.Authentication.GetAuthenticationInfo;
using BetterCms.Module.Root.Commands.Authentication.SearchRoles;
using BetterCms.Module.Root.Commands.Authentication.SearchUsers;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Autocomplete;

using Common.Logging;

using BetterModules.Core.Web.Models;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// User authentication handling controller.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(RootModuleDescriptor.RootAreaName)]
    public class AuthenticationController : CmsControllerBase
    {
        /// <summary>
        /// Current class logger.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

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
                Log.ErrorFormat("Failed to logout user {0}.", ex, User.Identity);
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
