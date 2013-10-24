using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Commands.User.GetUserProfile;
using BetterCms.Module.Users.Commands.User.SaveUserProfile;
using BetterCms.Module.Users.ViewModels.User;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User profile management.
    /// </summary>
    [BcmsAuthorize]
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class UserProfileController : CmsControllerBase
    {
        /// <summary>
        /// Edit the user profile.
        /// </summary>
        /// <returns>
        /// Edit user profile view.
        /// </returns>
        [HttpGet]
        public ActionResult EditProfile()
        {
            var model = GetCommand<GetUserProfileCommand>().ExecuteCommand(User.Identity.Name);
            var view = RenderView("EditUserProfile", model);

            return ComboWireJson(model != null, view, model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves the user profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Json status result
        /// </returns>
        [HttpPost]
        public ActionResult SaveUserProfile(EditUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = GetCommand<SaveUserProfileCommand>().ExecuteCommand(model);

                return WireJson(success);
            }

            return WireJson(false);
        }
    }
}