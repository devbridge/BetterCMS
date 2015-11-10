using System;
using System.Collections.Generic;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCMS.Module.LuceneSearch.Models
{
    public class IndexSource : EquatableEntity<IndexSource>, IDeleteableEntity
    {
        public virtual Guid SourceId { get; set; }

        public virtual string Path { get; set; }
        
        public virtual bool IsPublished { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? EndTime { get; set; }
        
        public virtual DateTime? NextRetryTime { get; set; }
        
        public virtual int FailedCount { get; set; }

        /// <summary>
        /// Compares two index sources based on the values of SourceId and Path properties.
        /// </summary>
        public class IndexSourceComparerByIdAndPath : IEqualityComparer<IndexSource>
        {
            /// <summary>
            /// Index sources are equal is both SourceId and Path are equal
            /// </summary>
            public bool Equals(IndexSource x, IndexSource y)
            {
                // Check whether the compared objects reference the same data. 
                if (Object.ReferenceEquals(x, y)) return true;

                // Check whether any of the compared objects is null. 
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                // Check whether the codes are equal. 
                return x.SourceId == y.SourceId && x.Path == y.Path;
            }

            /// <summary>
            /// If Equals() returns true for a pair of objects  then GetHashCode() must return the same value for these objects. 
            /// </summary>
            public int GetHashCode(IndexSource source)
            {
                // Check whether the object is null 
                if (Object.ReferenceEquals(source, null)) return 0;

                var sourceIdHash = source.SourceId == null ? 0 : source.SourceId.GetHashCode();
                var pathHash = source.Path == null ? 0 : source.Path.GetHashCode();

                return sourceIdHash ^ pathHash;
            }
        }
    }
}
