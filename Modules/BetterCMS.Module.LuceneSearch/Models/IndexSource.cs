// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexSource.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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
