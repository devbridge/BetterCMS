using System;

namespace BetterCms.Core.DataContracts
{
    /// <summary>
    /// Defines interface to access basic entity properties.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the entity id.
        /// </summary>
        /// <value>
        /// The entity id.
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
        /// Gets the created on date.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        DateTime CreatedOn { get; }

        /// <summary>
        /// Gets the modified on data.
        /// </summary>
        /// <value>
        /// The modified on.
        /// </value>
        DateTime ModifiedOn { get; }

        /// <summary>
        /// Gets the deleted on date.
        /// </summary>
        /// <value>
        /// The deleted on.
        /// </value>
        DateTime? DeletedOn { get; }

        /// <summary>
        /// Gets the created by user.
        /// </summary>
        /// <value>
        /// The created by user.
        /// </value>
        string CreatedByUser { get; }

        /// <summary>
        /// Gets the modified by user.
        /// </summary>
        /// <value>
        /// The modified by user.
        /// </value>
        string ModifiedByUser { get; }

        /// <summary>
        /// Gets the deleted by user.
        /// </summary>
        /// <value>
        /// The deleted by user.
        /// </value>
        string DeletedByUser { get; }

        /// <summary>
        /// Gets the entity version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        int Version { get; set; }
    }
}
