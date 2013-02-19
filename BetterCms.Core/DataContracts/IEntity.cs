using System;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic entity properties.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        DateTime CreatedOn { get; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>
        /// The modified on.
        /// </value>
        DateTime ModifiedOn { get; }

        /// <summary>
        /// Gets or sets the deleted on.
        /// </summary>
        /// <value>
        /// The deleted on.
        /// </value>
        DateTime? DeletedOn { get; }

        /// <summary>
        /// Gets or sets the created by user.
        /// </summary>
        /// <value>
        /// The created by user.
        /// </value>
        string CreatedByUser { get; }

        /// <summary>
        /// Gets or sets the modified by user.
        /// </summary>
        /// <value>
        /// The modified by user.
        /// </value>
        string ModifiedByUser { get; }

        /// <summary>
        /// Gets or sets the deleted by user.
        /// </summary>
        /// <value>
        /// The deleted by user.
        /// </value>
        string DeletedByUser { get; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; }
    }
}
