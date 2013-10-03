using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Commands.User.GetUserProfile
{
    public class GetUserProfileCommand : CommandBase, ICommand<string, EditUserProfileViewModel>
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserProfileCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public GetUserProfileCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public EditUserProfileViewModel Execute(string username)
        {
            var model = Repository.AsQueryable<Models.User>()
                    .Where(user => user.UserName == username)
                    .Select(
                        user =>
                            new
                            {
                                Salt = user.Salt,
                                Model = new EditUserProfileViewModel
                                {
                                    Version = user.Version,
                                    FirstName = user.FirstName,
                                    Email = user.Email,
                                    LastName = user.LastName,
                                    UserName = user.UserName,
                                    Image =
                                        new ImageSelectorViewModel
                                        {
                                            ImageId = user.Image.Id,
                                            ImageUrl = user.Image.PublicUrl,
                                            ThumbnailUrl = user.Image.PublicThumbnailUrl,
                                            ImageTooltip = user.Image.Caption,
                                            FolderId = user.Image.Folder != null ? user.Image.Folder.Id : (System.Guid?)null
                                        }
                                }
                            }
                        )
                    .FirstOne();

            model.Model.SecurityHash = authenticationService.CreatePasswordHash(username, model.Salt);

            return model.Model;
        }
    }
}