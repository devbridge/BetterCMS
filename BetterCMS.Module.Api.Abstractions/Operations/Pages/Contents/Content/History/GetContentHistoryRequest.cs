using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.History
{
    [Route("/contents/{ContentId}/history", Verbs = "GET")]
    [DataContract]
    public class GetContentHistoryRequest : RequestBase<GetContentHistoryModel>, IReturn<GetContentHistoryResponse>
    {
    }
}