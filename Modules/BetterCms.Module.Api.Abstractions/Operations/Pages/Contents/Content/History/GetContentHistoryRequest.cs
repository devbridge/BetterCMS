using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [Route("/contents/{ContentId}/history", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetContentHistoryRequest : IReturn<GetContentHistoryResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get; set;
        }
    }
}