using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [Route("/files", Verbs = "GET")]
    [DataContract]
    public class GetFilesRequest : RequestBase<GetFilesModel>, IReturn<GetFilesResponse>
    {
    }
}
