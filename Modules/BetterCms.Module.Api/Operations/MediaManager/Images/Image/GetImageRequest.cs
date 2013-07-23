using System.Runtime.Serialization;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    [Route("/images/{ImageId}", Verbs = "GET")]
    [DataContract]
    public class GetImageRequest : RequestBase<GetImageModel>, IReturn<GetImageResponse>
    {
    }
}