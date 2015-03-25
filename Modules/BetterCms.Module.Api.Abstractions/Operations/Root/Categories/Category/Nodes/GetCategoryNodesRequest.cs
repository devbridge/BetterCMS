using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Nodes
{
    /// <summary>
    /// Request to get category data.
    /// </summary>
    [Route("/categorytrees/{CategoryTreeId}/nodes/", Verbs = "GET")]
    [Route("/categorytrees/nodes/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetCategoryNodesRequest : RequestBase<DataOptions>, IReturn<GetCategoryNodesResponse>
    {
        [DataMember]
        public Guid? CategoryTreeId { get; set; }
    }
}