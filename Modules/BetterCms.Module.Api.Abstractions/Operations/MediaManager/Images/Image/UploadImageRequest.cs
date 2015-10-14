using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for upload image from the stream.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadImageRequest : RequestBase<UploadImageModel>
    {
    }
}
