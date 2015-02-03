using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category
{
    /// <summary>
    /// Request to get category data.
    /// </summary>
    [Route("/categorytrees/{CategoryTreeId}", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetCategoryTreeRequest : RequestBase<GetCategoryTreeModel>, IReturn<GetCategoryTreeResponse>
    {
        [DataMember]
        public Guid CategoryTreeId { get; set; }
    }
}