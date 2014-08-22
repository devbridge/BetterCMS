using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContent : EquatableEntity<ChildContent>, IChildContent
    {
        public virtual Content Parent { get; set; }

        public virtual Content Child { get; set; }
        
        public virtual Guid AssignmentIdentifier { get; set; }

        public virtual IList<ChildContentOption> Options { get; set; }

        IContent IChildContent.ChildContent
        {
            get
            {
                return Child;
            }
        }

        IEnumerable<IOption> IChildContent.Options
        {
            get
            {
                return Options;
            }
        }
    }
}