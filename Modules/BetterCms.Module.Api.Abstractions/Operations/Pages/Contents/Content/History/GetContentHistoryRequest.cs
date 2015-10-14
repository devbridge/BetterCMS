using System.Runtime.Serialization;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [DataContract]
    [System.Serializable]
    public class GetContentHistoryRequest
    {
        [DataMember]
        public System.Guid ContentId
        {
            get; set;
        }
    }
}