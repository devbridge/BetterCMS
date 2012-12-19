using System;

namespace BetterCms.Core.Models
{
    /// <summary>
    /// Specific logic to implement <see cref="System.IEquatable" /> interface.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>    
    [Serializable]
    public abstract class EquatableEntity<TEntity> : Entity, IEquatable<TEntity>
        where TEntity : Entity
    {
        private int? hashCode = null;

        public static bool operator ==(EquatableEntity<TEntity> x, EquatableEntity<TEntity> y)
        {
            return Equals(x as TEntity, y as TEntity);
        }

        public static bool operator !=(EquatableEntity<TEntity> x, EquatableEntity<TEntity> y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The object to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>        
        public virtual bool Equals(TEntity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            bool otherIsTransient = other.Id == default(Guid);
            bool thisIsTransient = Id == default(Guid);

            // If both object are new.
            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return Equals(other as TEntity);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            // Once e a hash code is generated we'll never change it in object life cycle.
            if (hashCode == null)
            {
                bool thisIsTransient = Id == default(Guid);

                if (thisIsTransient)
                {
                    hashCode = base.GetHashCode();
                }
                else
                {
                    hashCode = Id.GetHashCode();
                }
            }

            return hashCode.Value;
        }
    }
}
