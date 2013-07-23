using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [Route("/images", Verbs = "GET")]
    [DataContract]
    public class GetImagesRequest : RequestBase<GetImagesModel>, IReturn<GetImagesResponse>
    {
    }
}
