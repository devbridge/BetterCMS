using System;

using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.Root.Models
{
    /// <summary>
    /// PageAccess entity.
    /// </summary>
    [Serializable]
    public class PageAccess : EquatableEntity<PageAccess>, IAccess
    {
        private Guid objectId;

        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        /// <value>
        /// The object id.
        /// </value>
        public virtual Page Page { get; set; }

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

        /// <summary>
        /// Gets the object id.
        /// </summary>
        /// <value>
        /// The object id.
        /// </value>
        public virtual Guid ObjectId
        {
            get
            {
                return Page != null ? Page.Id : objectId;
            }
            set
            {
                objectId = value;

                Page = new Page {
                                    Id = value
                                };
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Page: {1}, RoleOrUser: {2}, AccessLevel: {3}", base.ToString(), Page, RoleOrUser, AccessLevel);
        }
    }
}