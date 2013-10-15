using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Commands.Registration;
using BetterCms.Module.Users.ViewModels.Registration;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User management.
    /// </summary>    
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class RegistrationController : CmsControllerBase
    {
        private readonly UsersModuleDescriptor usersModuleDescriptor;

        public RegistrationController(UsersModuleDescriptor usersModuleDescriptor)
        {
            this.usersModuleDescriptor = usersModuleDescriptor;
        }

        /// <summary>
        /// Creates the first user.
        /// </summary>        
        [HttpGet]
        public ActionResult CreateFirstUser()
        {
            if (usersModuleDescriptor.IsFirstUserRegistered)
            {
                throw new HttpException(403, "First user is already registered!");
            }

            return View(new CreateFirstUserViewModel());
        }

        /// <summary>
        /// Creates the first user.
        /// </summary>
        /// <param name="model">The model.</param>
        [HttpPost]
        public ActionResult CreateFirstUser(CreateFirstUserViewModel model)
        {
            if (usersModuleDescriptor.IsFirstUserRegistered)
            {
                throw new HttpException(403, "First user is already registered!");
            }

            if (ModelState.IsValid)
            {
                if (GetCommand<CreateFirstUserCommand>().ExecuteCommand(model))
                {
                    usersModuleDescriptor.SetAsFirstUserRegistered();

                    return Redirect(FormsAuthentication.LoginUrl ?? "/");
                }
            }

            return View(model);
        }
    }
}
