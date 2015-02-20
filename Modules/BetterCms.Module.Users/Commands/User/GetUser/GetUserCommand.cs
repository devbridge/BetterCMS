using System;
using System.Linq;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUser
{
    /// <summary>
    /// Command for getting user view model
    /// </summary>
    public class GetUserCommand : CommandBase, ICommand<Guid, EditUserViewModel>
    {
        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCommand"/> class.
        /// </summary>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetUserCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public EditUserViewModel Execute(Guid userId)
        {
            EditUserViewModel model;
            
            if (!userId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.User>()
                    .Where(bp => bp.Id == userId)
                    .Select(
                        user =>
                        new EditUserViewModel
                            {
                                Id = user.Id,
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
                                            FolderId = user.Image.Folder != null ? user.Image.Folder.Id : (Guid?)null
                                        } : null
                            })
                    .ToFuture();

                var roles = Repository
                    .AsQueryable<Models.UserRole>()
                    .Where(ur => ur.User.Id == userId)
                    .Select(ur => ur.Role.Name)
                    .ToFuture();

                model = listFuture.FirstOne();
                model.Roles = roles.ToList();
            }
            else
            {
                model = new EditUserViewModel();
            }

            return model;
        }
    }
}