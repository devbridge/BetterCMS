using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Uploading image response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class UploadImageResponse : SaveResponseBase
    {
    }
}
