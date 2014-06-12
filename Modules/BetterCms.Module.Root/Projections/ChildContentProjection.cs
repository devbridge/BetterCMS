using System.Collections.Generic;
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    public class ChildContentProjection : PageContentProjection
    {
        private readonly System.Guid childContentId;

        public ChildContentProjection(IPageContent pageContent, IChildContent content, IContentAccessor contentAccessor, IList<ChildContentProjection> childProjections = null)
            : base(pageContent, content.ChildContent, contentAccessor, childProjections)
        {
            childContentId = content.Id;
        }

        public ChildContentProjection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public System.Guid ChildContentId
        {
            get
            {
                return childContentId;
            }
        }
    }
}