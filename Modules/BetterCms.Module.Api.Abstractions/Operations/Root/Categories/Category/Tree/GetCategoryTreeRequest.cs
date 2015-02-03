using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Root.Categories.Category.Tree
{
    [Route("/categories/{CategoryId}/tree/", Verbs = "GET")]
    [Serializable]
    [DataContract]
    public class GetCategoryTreeRequest : RequestBase<GetCategoryTreeModel>, IReturn<GetCategoryTreeResponse>
    {
        [DataMember]
        public Guid CategoryId { get; set; }
    }
}