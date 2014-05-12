using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images
{
    /// <summary>
    /// Image creation response.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PostImagesResponse : ResponseBase<Guid?>
    {
    }
}