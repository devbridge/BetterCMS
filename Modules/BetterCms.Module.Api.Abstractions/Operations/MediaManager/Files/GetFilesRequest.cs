using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [Route("/files", Verbs = "GET")]
    [DataContract]
    [System.Serializable]
    public class GetFilesRequest : RequestBase<GetFilesModel>, IReturn<GetFilesResponse>
    {
    }
}
