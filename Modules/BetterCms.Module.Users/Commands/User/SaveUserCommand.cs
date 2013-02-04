using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels;

namespace BetterCms.Module.Users.Commands.User
{
    /// <summary>
    /// A command to save user item.
    /// </summary>
    public class SaveUserCommand: CommandBase, ICommand<EditUserViewModel, SaveUserResponse>
    {
        /// <summary>
        /// Executes a command to save user.
        /// </summary>
        /// <param name="userItem">The user item.</param>
        /// <returns>
        /// true if user saved successfully; false otherwise.
        /// </returns>
        public SaveUserResponse Execute(EditUserViewModel userItem)
        {
            UnitOfWork.BeginTransaction();

            var user = !userItem.Id.HasDefaultValue()
                               ? Repository.AsQueryable<Models.Users>()
                                           .Where(f => f.Id == userItem.Id)
                                           .ToList()
                                           .FirstOrDefault()
                               : new Models.Users();

            if (user == null)
            {
                user = new Models.Users();
            }

            user.UserName = userItem.UserName;
            user.FirstName = userItem.FirstName;
            user.LastName = userItem.LastName;
            user.Email = userItem.Email;
            user.Password = userItem.Password;
            user.Version = userItem.Version;

            if (userItem.Image != null && userItem.Image.ImageId.HasValue)
            {
                user.Image = Repository.AsProxy<MediaImage>(userItem.Image.ImageId.Value);
            }
            else
            {
                user.Image = null;
            }

            Repository.Save(user);
            UnitOfWork.Commit();

            return new SaveUserResponse() { Id = user.Id, UserName = user.UserName, Version = user.Version };
        }
    }
}