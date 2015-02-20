using System.Linq;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.User.GetUserProfile
{
    public class GetUserProfileCommand : CommandBase, ICommand<string, EditUserProfileViewModel>
    {
        /// <summary>
        /// The authentication service
        /// </summary>
        private IAuthenticationService authenticationService;

        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserProfileCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetUserProfileCommand(IAuthenticationService authenticationService, IMediaFileUrlResolver fileUrlResolver)
        {
            this.authenticationService = authenticationService;
            this.fileUrlResolver = fileUrlResolver;
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
                                    Image = user.Image != null && !user.Image.IsDeleted ?
                                        new ImageSelectorViewModel
                                        {
                                            ImageId = user.Image.Id,
                                            ImageUrl = fileUrlResolver.EnsureFullPathUrl(user.Image.PublicUrl),
                                            ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(user.Image.PublicThumbnailUrl),
                                            ImageTooltip = user.Image.Caption,
                                            FolderId = user.Image.Folder != null ? user.Image.Folder.Id : (System.Guid?)null
                                        } : null
                                }
                            }
                        )
                    .FirstOne();

            model.Model.SecurityHash = authenticationService.CreatePasswordHash(username, model.Salt);

            return model.Model;
        }
    }
}