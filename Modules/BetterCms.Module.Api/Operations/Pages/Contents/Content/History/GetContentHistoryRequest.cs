using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [Route("/contents/{ContentId}/history", Verbs = "GET")]
    [DataContract]
    public class GetContentHistoryRequest : RequestBase<GetContentHistoryModel>, IReturn<GetContentHistoryResponse>
    {
        [DataMember]
        public System.Guid ContentId
        {
            get
            {
                return Data.ContentId;
            }
            set
            {
                Data.ContentId = value;
            }
        }
    }
}