using System;

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
    }
}
