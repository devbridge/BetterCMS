using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

using ServiceStack.ServiceHost;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// Request for image update or creation.
    /// </summary>
    [Route("/images", Verbs = "POST")]
    [DataContract]
    [Serializable]
    public class PostImagesRequest : RequestBase<Image.SaveImageModel>, IReturn<PostImagesResponse>
    {
    }
}