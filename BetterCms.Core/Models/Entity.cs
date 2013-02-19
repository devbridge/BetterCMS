using System;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Implements generic entities interface.
    /// </summary>
    [Serializable]
    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public virtual DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the modified on.
        /// </summary>
        /// <value>
        /// The modified on.
        /// </value>
        public virtual DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the deleted on.
        /// </summary>
        /// <value>
        /// The deleted on.
        /// </value>
        public virtual DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Gets or sets the created by user.
        /// </summary>
        /// <value>
        /// The created by user.
        /// </value>
        public virtual string CreatedByUser { get; set; }

        /// <summary>
        /// Gets or sets the modified by user.
        /// </summary>
        /// <value>
        /// The modified by user.
        /// </value>
        public virtual string ModifiedByUser { get; set; }

        /// <summary>
        /// Gets or sets the deleted by user.
        /// </summary>
        /// <value>
        /// The deleted by user.
        /// </value>
        public virtual string DeletedByUser { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public virtual int Version { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Id={1}, IsDeleted={2}, Version={3}", GetType().Name, Id, IsDeleted, Version);
        }
    }
}