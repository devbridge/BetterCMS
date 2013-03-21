using System;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Models.Authentication;
using BetterCms.Module.Root.Mvc;

using Common.Logging;

namespace BetterCms.Module.Root.Controllers
{
    /// <summary>
    /// User authentication handling controller.
    /// </summary>
    [BcmsAuthorize]
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
        public ActionResult Info()
        {
            InfoViewModel model = new InfoViewModel();
            model.IsUserAuthenticated = User.Identity.IsAuthenticated;
            model.UserName = User.Identity.Name;

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
                FormsAuthentication.SignOut();         
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Failed to logout user {0}.", ex, User.Identity);
            }

            return Redirect(FormsAuthentication.DefaultUrl);
        }
    }
}
