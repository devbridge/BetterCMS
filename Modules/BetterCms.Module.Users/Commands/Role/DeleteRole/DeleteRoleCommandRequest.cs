using System;

namespace BetterCms.Module.Users.Commands.Role.DeleteRole
{
    /// <summary>
    /// Data contract for DeleteRoleCommand.
    /// </summary>
    public class DeleteRoleCommandRequest
    {
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>
        /// The role id.
        /// </value>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public int Version { get; set; }
    }
}