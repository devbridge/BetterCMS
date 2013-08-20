using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels;

namespace BetterCms.Module.Users.Commands.User.GetUser
{
    /// <summary>
    /// Command for getting user view model
    /// </summary>
    public class GetUserCommand : CommandBase, ICommand<Guid, EditUserViewModel>
    {
        /// <summary>
        /// The role service
        /// </summary>
        private IRoleService roleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCommand" /> class.
        /// </summary>
        /// <param name="roleService">The role service.</param>
        public GetUserCommand(IRoleService roleService)
        {
            this.roleService = roleService;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="id">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public EditUserViewModel Execute(Guid userId)
        {
            var model = new EditUserViewModel();

            if (!userId.HasDefaultValue())
            {
                model =
                    Repository.AsQueryable<Models.Users>()
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
                              .FirstOrDefault();
            }
            if (model != null)
            {
                model.Roles = roleService.GetUserRoles();
            }

            return model;
        }
    }
}