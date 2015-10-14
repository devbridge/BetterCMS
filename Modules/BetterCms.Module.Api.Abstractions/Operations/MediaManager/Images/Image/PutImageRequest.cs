using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Request for tag update or creation.
    /// </summary>
    [DataContract]
    [Serializable]
    public class PutImageRequest : PutRequestBase<SaveImageModel>
    {
    }
}