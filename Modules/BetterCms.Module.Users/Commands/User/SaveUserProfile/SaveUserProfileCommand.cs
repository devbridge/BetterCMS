using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.SaveUserProfile
{
    public class SaveUserProfileCommand : CommandBase, ICommand<EditUserProfileViewModel, bool>
    {
        public bool Execute(EditUserProfileViewModel request)
        {
            var message = "TODO: implement user profile loading and saving.";

            throw new ValidationException(() => message, message);
        }
    }
}