using System.Web.Security;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;

namespace BetterCms.Module.Users.Commands.Authentication
{
    public class LogoutCommand : CommandBase, ICommand
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public LogoutCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        public void Execute()
        {
            FormsAuthentication.SignOut();            
        }
    }
}