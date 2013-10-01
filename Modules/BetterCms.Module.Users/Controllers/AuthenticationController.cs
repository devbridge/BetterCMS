using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Commands.Authentication;
using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Authentication;

using Common.Logging;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User management.
    /// </summary>    
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class AuthenticationController : CmsControllerBase
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Creates the first user.
        /// </summary>        
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            if (!FormsAuthentication.IsEnabled)
            {
                Messages.AddError(UsersGlobalization.Login_FormsAuthentication_DisabledMessage);
            }

            if (User.Identity.IsAuthenticated)
            {
                return Redirect(FormsAuthentication.DefaultUrl ?? "/");
            }

            return View(new LoginViewModel
                            {
                                ReturnUrl = returnUrl,
                                IsFormsAuthenticationEnabled = FormsAuthentication.IsEnabled
                            });
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!FormsAuthentication.IsEnabled)
            {
                Messages.AddError(string.Empty, UsersGlobalization.Login_FormsAuthentication_DisabledMessage);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    HttpCookie authCookie = GetCommand<LoginCommand>().ExecuteCommand(model);
                    if (authCookie != null)
                    {
                        Response.Cookies.Add(authCookie);

                        return Redirect(model.ReturnUrl ?? (FormsAuthentication.DefaultUrl ?? "/"));
                    }
                }
            }
            model.IsFormsAuthenticationEnabled = FormsAuthentication.IsEnabled;
            return View(model);
        }

        [HttpGet]
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
    }
}
