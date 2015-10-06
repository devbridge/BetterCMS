using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for image update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class DeleteImageRequest : DeleteRequestBase
    {
    }
}