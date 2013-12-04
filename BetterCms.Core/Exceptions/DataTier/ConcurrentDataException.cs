using System;
using BetterCms.Core.Models;

namespace BetterCms.Core.Exceptions.DataTier
{
    /// <summary>
    /// Concurrent data exception
    /// </summary>
    [Serializable]
    public class ConcurrentDataException : DataTierException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ConcurrentDataException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConcurrentDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConcurrentDataException" /> class.
        /// </summary>
        /// <param name="staleEntity">The stale entity.</param>
        public ConcurrentDataException(Entity staleEntity)
            : base(string.Format("{0} with id {1} Entity was updated by another transaction.", staleEntity.GetType().Name, staleEntity.Id))
        {
            StaleEntity = staleEntity;
        }

        /// <summary>
        /// Gets or sets the stale entity.
        /// </summary>
        public Entity StaleEntity { get; set; }
    }
}
