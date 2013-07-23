using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents
{
    [Route("/pages/{PageId}/contents")]
    [DataContract]
    public class GetPageContentsRequest : RequestBase<GetPageContentsModel>, IReturn<GetPageContentsResponse>
    {
    }
}