using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Route("/categorytrees/{CategoryTreeId}/tree/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetNodesTreeRequest : RequestBase<GetNodesTreeModel>, IReturn<GetNodesTreeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}