using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [Route("/images/{Id}", Verbs = "PUT")]
    [DataContract]
    [Serializable]
    public class PutImageRequest : PutRequestBase<SaveImageModel>, IReturn<PutImageResponse>
    {
    }
}