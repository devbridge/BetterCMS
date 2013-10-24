using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Users.Commands.User.DeleteUser
{
    /// <summary>
    /// A command to delete given user.
    /// </summary>
    public class DeleteUserCommand : CommandBase, ICommand<DeleteUserCommandRequest, bool>
    {
        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Executed command result.</returns>
        public bool Execute(DeleteUserCommandRequest request)
        {
            var user = Repository.First<Models.User>(request.UserId);
            user.Version = request.Version;
            Repository.Delete(user);

            if (user.UserRoles != null)
            {
                foreach (var userRole in user.UserRoles)
                {
                    Repository.Delete(userRole);
                }
            }

            UnitOfWork.Commit();

            // Notify.
            Events.UserEvents.Instance.OnUserDeleted(user);

            return true;
        }
    }
}