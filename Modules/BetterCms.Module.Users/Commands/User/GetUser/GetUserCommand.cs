using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;

using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.User;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUser
{
    /// <summary>
    /// Command for getting user view model
    /// </summary>
    public class GetUserCommand : CommandBase, ICommand<Guid, EditUserViewModel>
    {
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
                        bp =>
                        new EditUserViewModel
                            {
                                Id = bp.Id,
                                Version = bp.Version,
                                FirstName = bp.FirstName,
                                Email = bp.Email,
                                LastName = bp.LastName,
                                UserName = bp.UserName,
                                Image =
                                    new ImageSelectorViewModel
                                        {
                                            ImageId = bp.Image.Id,
                                            ImageUrl = bp.Image.PublicUrl,
                                            ThumbnailUrl = bp.Image.PublicThumbnailUrl,
                                            ImageTooltip = bp.Image.Caption
                                        }
                            })
                    .ToFuture();

                var roles = Repository
                    .AsQueryable<Models.UserRole>()
                    .Where(ur => ur.User.Id == userId)
                    .Select(ur => ur.Role.DisplayName ?? ur.Role.Name)
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