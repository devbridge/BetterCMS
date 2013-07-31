using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.MediaManager.Files
{
    [DataContract]
    public class GetFilesResponse : ListResponseBase<MediaModel>
    {
    }
}