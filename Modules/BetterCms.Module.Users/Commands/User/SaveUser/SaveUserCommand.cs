using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.User.SaveUser
{
    /// <summary>
    /// A command to save user item.
    /// </summary>
    public class SaveUserCommand : CommandBase, ICommand<EditUserViewModel, SaveUserCommandResponse>
    {
        /// <summary>
        /// The user service
        /// </summary>
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveUserCommand" /> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public SaveUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// Executes a command to save user.
        /// </summary>
        /// <param name="request">The user item.</param>
        /// <returns>
        /// true if user saved successfully; false otherwise.
        /// </returns>
        public SaveUserCommandResponse Execute(EditUserViewModel request)
        {
            var user = userService.SaveUser(request);

            return new SaveUserCommandResponse
                       {
                           Id = user.Id, 
                           UserName = user.UserName, 
                           Version = user.Version,
                           Email = user.Email,
                           FullName = user.FirstName + " " + user.LastName
                       };
        }
    }
}