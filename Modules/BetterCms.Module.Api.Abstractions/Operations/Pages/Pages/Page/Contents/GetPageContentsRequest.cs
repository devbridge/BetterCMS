using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [Route("/pages/{PageId}/contents")]
    [DataContract]
    [Serializable]
    public class GetPageContentsRequest : RequestBase<GetPageContentsModel>, IReturn<GetPageContentsResponse>
    {
        [DataMember]
        public Guid PageId { get; set; }
    }
}