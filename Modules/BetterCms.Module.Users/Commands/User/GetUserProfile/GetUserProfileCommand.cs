using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.GetUserProfile
{
    public class GetUserProfileCommand : CommandBase, ICommand<string, EditUserProfileViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public EditUserProfileViewModel Execute(string username)
        {
            return new EditUserProfileViewModel();
        }
    }
}