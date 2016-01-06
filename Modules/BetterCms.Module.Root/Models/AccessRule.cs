using System;

using BetterCms.Core.Security;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// PageAccess entity.
    /// </summary>
    [Serializable]
    public class AccessRule : EquatableEntity<AccessRule>, IAccessRule, IDeleteableEntity
    {
        /// <summary>
        /// Gets or sets the user/role.
        /// </summary>
        /// <value>
        /// The user/role.
        /// </value>
        public virtual string Identity { get; set; }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        public virtual AccessLevel AccessLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this rule applies for role.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance applies for role; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsForRole { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Identity: {1}, AccessLevel: {2}", base.ToString(), Identity, AccessLevel);
        }
    }
}