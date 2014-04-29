using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Images.Image
{
    /// <summary>
    /// Response after image saving.
    /// </summary>
    [DataContract]
    public class PutImageResponse : ResponseBase<Guid?>
    {
    }
}