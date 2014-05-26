using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    [Route("/images", Verbs = "GET")]
    [DataContract]
    [Serializable]
    public class GetImagesRequest : RequestBase<GetImagesModel>, IReturn<GetImagesResponse>
    {
    }
}
