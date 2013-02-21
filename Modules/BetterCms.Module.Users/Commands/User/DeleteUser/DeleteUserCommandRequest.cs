using System;

namespace BetterCms.Module.Users.Commands.User.DeleteUser
{
    /// <summary>
    /// Data contract for DeleteUserCommand.
    /// </summary>
    public class DeleteUserCommandRequest
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>
        /// The user id.
        /// </value>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}