using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [DataContract]
    [Serializable]
    public class GetFilesResponse : ListResponseBase<MediaModel>
    {
    }
}