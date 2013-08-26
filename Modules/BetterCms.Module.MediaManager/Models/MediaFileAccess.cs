using System;

using BetterCms.Core.Models;
using BetterCms.Core.Security;

namespace BetterCms.Module.MediaManager.Models
{
    /// <summary>
    /// MediaFileAccess entity.
    /// </summary>
    [Serializable]
    public class MediaFileAccess : EquatableEntity<MediaFileAccess>, IAccess
    {
        /// <summary>
        /// Gets or sets the object id.
        /// </summary>
        /// <value>
        /// The object id.
        /// </value>
        public virtual MediaFile MediaFile { get; set; }

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
        Guid IAccess.ObjectId
        {
            get
            {
                return MediaFile != null ? MediaFile.Id : Guid.Empty;
            }
            set
            {
                MediaFile = new MediaFile {
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
            return string.Format("{0}, MediaFile: {1}, RoleOrUser: {2}, AccessLevel: {3}", base.ToString(), MediaFile, RoleOrUser, AccessLevel);
        }
    }
}