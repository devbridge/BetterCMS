using System;

using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// UserAccess entity.
    /// </summary>
    [Serializable]
    public class UserAccess : EquatableEntity<UserAccess>, IUserAccess
    {
        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        /// <value>
        /// The object id.
        /// </value>
        public virtual Guid ObjectId { get; set; }

        /// <summary>
        /// Gets or sets the user/role.
        /// </summary>
        /// <value>
        /// The user/role.
        /// </value>
        public virtual string RoleOrUser { get; set; }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        public virtual AccessLevel AccessLevel { get; set; }
    }
}