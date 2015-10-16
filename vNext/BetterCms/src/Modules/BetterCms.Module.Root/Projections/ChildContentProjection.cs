using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    public class ChildContentProjection : PageContentProjection
    {
        private readonly System.Guid assignmentIdentifier;

        public ChildContentProjection(IPageContent pageContent, IChildContent content, IContentAccessor contentAccessor,
            IEnumerable<ChildContentProjection> childProjections = null, IEnumerable<PageContentProjection> childRegionContentProjections = null)
            : base(pageContent, content.ChildContent, contentAccessor, childProjections, childRegionContentProjections)
        {
            assignmentIdentifier = content.AssignmentIdentifier;
        }

        public ChildContentProjection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        
        public System.Guid AssignmentIdentifier
        {
            get
            {
                return assignmentIdentifier;
            }
        }
    }
}